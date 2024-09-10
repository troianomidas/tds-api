using WebApi.Domain.Common;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities.Collaborators;
using WebApi.Domain.Entities.Stores;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class Order : BaseStoreEntity
{
    public Order()
    {
        Items = new List<OrderItem>();
    }

    public Order(int storeId, int deliveryTypeId, List<OrderItem>? items)
    {
        if (storeId < 1)
            throw new InvalidOperationException("StoreId is required.");

        if (deliveryTypeId is < 1 or > 3)
            throw new InvalidOperationException("DeliveryTypeId is invalid.");

        if (items == null || items.Count == 0)
            throw new InvalidOperationException("Por favor, adicione pelo menos 1 item ao pedido.");

        DateTime dateNow = DateTimeUtils.Now();
        
        StoreId = storeId;
        TrackId = Convert.ToInt64(
            $"{dateNow:ss}{dateNow:HH}{dateNow:MM}{dateNow:yy}{dateNow:dd}{new Random().Next(111, 999)}");
        DeliveryTypeId = deliveryTypeId;
        Items = items;
        Status = 1;
        CreatedAt = dateNow;
    }
    
    public long TrackId { get; set; }
    public int Number { get; set; }
    public int? PaymentMethodId { get; set; }
    public int DeliveryTypeId { get; set; }
    public int? CollaboratorId { get; set; }
    public bool IsScheduled { get; set; }
    public decimal ItemsValue { get; set; }
    public decimal DeliveryValue { get; set; }
    public decimal DiscountValue { get; set; }
    public int DiscountType { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime? DeliveryEstimateBeginAt { get; set; }
    public DateTime? DeliveryEstimateEndAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? TableReference { get; set; }
    public string? Obs { get; set; }
    public int Status { get; set; }
    public bool IsOnlineMenu { get; set; }
    public string? UserExternalId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public List<OrderItem> Items { get; set; }
    public virtual Review? Review { get; set; }
    public virtual Store? Store { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
    public virtual ShippingAddress? ShippingAddress { get; set; }
    public virtual Collaborator? Collaborator { get; set; }

    public void CalculateBalance()
    {
        ItemsValue = 0;
        TotalValue = 0;

        foreach (OrderItem item in Items)
        {
            if (item.ExternalParentId != null)
                continue;

            decimal value = item.UnitValue * item.Amount;

            foreach (OrderItem extra in Items.Where(x => x.ExternalParentId == item.ExternalId))
            {
                decimal extraValue = extra.UnitValue * extra.Amount;
                value += extraValue * item.Amount;
            }

            ItemsValue += value;
        }

        TotalValue += ItemsValue;
        TotalValue += DeliveryValue;

        if (DiscountValue > 0 && DiscountType == OrderDiscountTypeConst.Percentage)
            TotalValue -= (DiscountValue * TotalValue) / 100;
        if (DiscountValue > 0 && DiscountType == OrderDiscountTypeConst.Value)
            TotalValue -= DiscountValue;
    }

    public void ValidationPersistence()
    {
        if (Status is < 1 or > 9)
            throw new InvalidOperationException("Status do pedido invalido.");

        if (DeliveryValue is < 0 or > 1000000)
            throw new InvalidOperationException("Valor da entrega invalido.");

        if ((DiscountType < 0 || DiscountType > 3) || DiscountValue < 0)
            throw new InvalidOperationException("Valor do desconto invalido.");

        if (DeliveryEstimateBeginAt == null || DeliveryEstimateBeginAt == DateTime.MinValue)
            throw new InvalidOperationException("Por favor, informe uma 'Data de Entrega' valida do pedido.");

        if (DeliveryEstimateEndAt == null || DeliveryEstimateEndAt == DateTime.MinValue)
            throw new InvalidOperationException("Por favor, informe uma 'Data de Entrega' valida do pedido.");

        if (DeliveryEstimateEndAt <= DeliveryEstimateBeginAt)
            throw new InvalidOperationException("Por favor, informe uma 'Data de Entrega' valida do pedido.");
        
        if (DeliveryTypeId == OrderDeliveryTypeConst.Delivery)
        {
            if (string.IsNullOrEmpty(ShippingAddress?.Line1))
                throw new InvalidOperationException("Por favor, informe endereco para entrega do pedido.");
            
            if (string.IsNullOrEmpty(ShippingAddress?.Number))
                throw new InvalidOperationException("Por favor, informe endereco para entrega do pedido.");

            if (string.IsNullOrEmpty(ShippingAddress?.Neighborhood))
                throw new InvalidOperationException("Por favor, informe endereco para entrega do pedido.");
        }
        else
            if (DeliveryValue > 0)
                throw new InvalidOperationException("Pedido nao pode ter valor para entrega");

        if (DiscountType == OrderDiscountTypeConst.Percentage && DiscountValue is < 0 or > 100)
            throw new InvalidOperationException("Porcentagem para desconto invalido.");

        if (DiscountType == OrderDiscountTypeConst.Value && DiscountValue < 0 || DiscountValue > TotalValue)
            throw new InvalidOperationException("Valor para desconto invalido.");

        if (ItemsValue is < 1 or > 1000000)
            throw new InvalidOperationException("Valor dos itens invalido.");

        if (TotalValue is < 1 or > 1000000)
            throw new InvalidOperationException("Valor do pedido invalido.");
    }
}