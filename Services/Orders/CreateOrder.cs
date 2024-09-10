using Azure.Storage.Queues;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Integrations.Queues;
using WebApi.Integrations.Serverless;
using WebApi.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace WebApi.Services.Orders;

public record CreateOrderRequest : IRequest<Order>
{
    public int StoreId { get; set; }
    public long TrackId { get; set; }
    public int? PaymentMethodId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? UserExternalId { get; set; }
    public int Status { get; set; }
    public bool IsOnlineMenu { get; set; }
    public decimal DeliveryValue { get; set; }
    public decimal DiscountValue { get; set; }
    public int DiscountType { get; set; }
    public bool IsScheduled { get; set; }
    public DateTime DeliveryEstimateBeginAt { get; set; }
    public DateTime DeliveryEstimateEndAt { get; set; }
    public int DeliveryTypeId { get; set; }
    public string? TableReference { get; set; }
    public ShippingAddress? ShippingAddress { get; set; }
    public List<OrderItem>? Items { get; set; }
}

public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly AppDbContext _context;
    private readonly IDelivery3Serverless _serverless;

    public CreateOrderRequestHandler(AppDbContext context, IDelivery3Serverless serverless)
    {
        _context = context;
        _serverless = serverless;
    }

    public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order(request.StoreId, request.DeliveryTypeId, request.Items)
        {
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            UserExternalId = request.UserExternalId,
            PaymentMethodId = request.PaymentMethodId,
            DeliveryEstimateBeginAt = request.DeliveryEstimateBeginAt,
            DeliveryEstimateEndAt = request.DeliveryEstimateEndAt,
            DeliveryValue = request.DeliveryValue,
            DiscountValue = request.DiscountValue,
            DiscountType = request.DiscountType,
            IsScheduled = request.IsScheduled,
            TableReference = request.TableReference,
            ShippingAddress = request.ShippingAddress,
            Status = request.Status,
            IsOnlineMenu = request.IsOnlineMenu,
        };

        if (request.IsOnlineMenu)
        {
            order.TrackId = request.TrackId;
            
            if (string.IsNullOrEmpty(order.CustomerName) || string.IsNullOrEmpty(order.CustomerPhone))
                throw new InvalidOperationException("Por favor, preencha os campos de Nome e/ou Celular.", new Exception("name"));
        }

        if (request.DeliveryTypeId != OrderDeliveryTypeConst.Delivery)
        {
            order.ShippingAddress = null;
            order.DeliveryValue = 0;
        }
        
        order.CalculateBalance();
        order.ValidationPersistence();

        _context.Orders.Add(order);

        await _context.SaveChangesAsync(cancellationToken);

        if (order is { IsOnlineMenu: true, Status: OrderStatusConst.Pending })
            await _serverless.RefreshWorkflowOrderAsync(order.StoreId);
        
        return order;
    }
}