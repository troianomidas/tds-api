using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services.Customers;

public class ListCustomerBalanceRequest : IRequest<List<CustomerBalancePointsView>>
{
    public int StoreId { get; set; }
    public string? Filter { get; set; }
}

public class
    ListCustomerBalanceRequestHandler : IRequestHandler<ListCustomerBalanceRequest, List<CustomerBalancePointsView>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListCustomerBalanceRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CustomerBalancePointsView>> Handle(ListCustomerBalanceRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Filter.IsNullOrEmpty())
            throw new BadRequestException("erro");
        
        if (request.Filter is { Length: < 6 })
            throw new BadRequestException("erro");
        
        IQueryable<CustomerBalancePointsView> query =
            _context.CustomerBalancePointsVw.Where(x => x.StoreId == request.StoreId &&
                  x.Name.StartsWith(request.Filter!.ToUpper()));

        if (request.Filter != null && request.Filter.All(char.IsDigit))
            query = _context.CustomerBalancePointsVw
                .Where(x => x.StoreId == request.StoreId && x.Phone == Convert.ToInt64(request.Filter));
            
        return await query.Take(15).ToListAsync(cancellationToken);
    }
}