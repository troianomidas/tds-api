using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;

namespace WebApi.Services.Products;

public record ListProductsRequest : IRequest<List<Product>>
{
    public int StoreId { get; init; }
    public string? Filter { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class ListProductsHandler : IRequestHandler<ListProductsRequest, List<Product>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListProductsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Product>> Handle(ListProductsRequest request, CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");
        
        IQueryable<Product> query = _context.Products.Where(x => x.StoreId == request.StoreId && x.Status != 9);

        if (!string.IsNullOrEmpty(request.Filter))
            query = query.Where(x => x.Name != null && x.Name.ToLower().StartsWith(request.Filter.ToLower()));

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }
}