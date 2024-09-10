using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services.Customers;

public class ListCustomer : IRequest<List<Customer>>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
}

public class ListCustomerHandler : IRequestHandler<ListCustomer, List<Customer>>
{
    private readonly AppDbContext _context;

    public ListCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> Handle(ListCustomer request,
        CancellationToken cancellationToken)
    {
        if (request.Name.IsNullOrEmpty() && request.Phone.IsNullOrEmpty())
            throw new InvalidOperationException("'Nome' ou 'Celular' é obrigatório para buscar o cliente.");

        IQueryable<Order>? query;

        query = !string.IsNullOrEmpty(request.Name)
            ? _context.Orders.Where(x =>
                x.StoreId == request.StoreId && x.CustomerName != null && x.CustomerName.StartsWith(request.Name.ToUpper()))
            : _context.Orders.Where(x => x.StoreId == request.StoreId && x.CustomerPhone != null && x.CustomerPhone == request.Phone);
        
        var orders = await query.Take(5).ToListAsync(cancellationToken);
        return orders.Select(x => new Customer(x.StoreId, x.CustomerName, x.CustomerPhone)).ToList();
    }
}