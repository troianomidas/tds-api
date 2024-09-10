using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record GetPaymentMethodsRequest : IRequest<PaymentMethodResponse>
{
    public int StoreId { get; set; }
}

public class GetPaymentMethodsRequestHandler : IRequestHandler<GetPaymentMethodsRequest, PaymentMethodResponse>
{
    private readonly AppDbContext _context;

    public GetPaymentMethodsRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMethodResponse> Handle(GetPaymentMethodsRequest request, CancellationToken cancellationToken)
    {
        List<PaymentMethod> allPaymentMethods = await _context.PaymentMethods.ToListAsync(cancellationToken);
        List<StorePaymentMethod> paymentMethods = await _context.StorePaymentMethods.Where(x=> x.StoreId == request.StoreId).ToListAsync(cancellationToken);

        return new PaymentMethodResponse
        {
            AllPaymentMethods = allPaymentMethods,
            PaymentMethods = paymentMethods,
        };
    }
}

public class PaymentMethodResponse
{
    public List<PaymentMethod>? AllPaymentMethods { get; set; }
    public List<StorePaymentMethod>? PaymentMethods { get; set; }
}