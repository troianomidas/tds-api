using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record CreateOrUpdatePaymentMethodRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<CreateOrUpdatePaymentMethodItemRequest>? PaymentMethods { get; set; }

    public class CreateOrUpdatePaymentMethodItemRequest
    {
        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

public class CreateOrUpdatePaymentMethodRequestHandler : IRequestHandler<CreateOrUpdatePaymentMethodRequest, bool>
{
    private readonly AppDbContext _context;

    public CreateOrUpdatePaymentMethodRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateOrUpdatePaymentMethodRequest request, CancellationToken cancellationToken)
    {
        List<StorePaymentMethod> storePaymentMethodsDb = await _context.StorePaymentMethods
            .Where(x => x.StoreId == request.StoreId)
            .ToListAsync(cancellationToken);

        if (request.PaymentMethods == null)
            return false;

        foreach (StorePaymentMethod item in storePaymentMethodsDb)
        {
            if (request.PaymentMethods.Count(x => x.Id == item.Id) == 0)
                _context.StorePaymentMethods.Remove(item);
        }

        List<StorePaymentMethod> paymentMethods = new();

        foreach (CreateOrUpdatePaymentMethodRequest.CreateOrUpdatePaymentMethodItemRequest item in request.PaymentMethods)
        {
            if (item.Id > 0)
            {
                if (!string.IsNullOrEmpty(item.Name))
                {
                    storePaymentMethodsDb.First(x => x.Id == item.Id).Name = item.Name;
                    storePaymentMethodsDb.First(x => x.Id == item.Id).Description = item.Description;
                }
                
                continue;
            }
                

            paymentMethods.Add(new StorePaymentMethod
                { StoreId = request.StoreId, PaymentMethodId = item.PaymentMethodId, Name = item.Name, Description = item.Description});
        }

        _context.StorePaymentMethods.AddRange(paymentMethods);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}