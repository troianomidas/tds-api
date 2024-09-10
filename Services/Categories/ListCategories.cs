using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Categories;

public record ListCategoriesRequest : IRequest<List<ProductCategory>>
{
    public int StoreId { get; set; }
    public string? StoreHostname { get; set; }
}

public class
    ListCategoriesRequestHandler : IRequestHandler<ListCategoriesRequest, List<ProductCategory>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListCategoriesRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductCategory>> Handle(ListCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.StoreHostname) && request.StoreId > 0)
        {
            return await _context.Categories.Where(x => x.StoreId == request.StoreId && x.Status == 1)
                .Include(x => x.Products.Where(product => product.Status != 9))
                .ThenInclude(x=> x.ProductExtraMatches)!
                .ThenInclude(x=> x.ProductExtra)
                .ThenInclude(x=> x!.Items)
                .Include(x => x.Products.Where(product => product.Status != 9))
                .OrderBy(x => x.Sort)
                .ToListAsync(cancellationToken);
        }

        return await _context.Categories.Where(x => x.Store!.Hostname == request.StoreHostname && x.Status == 1)
            .Include(x => x.Products.Where(product => product.Status != 9))
            .ThenInclude(x=> x.ProductExtraMatches)!
            .ThenInclude(x=> x.ProductExtra)
            .ThenInclude(x=> x!.Items)
            .Include(x => x.Products.Where(product => product.Status != 9))
            .OrderBy(x => x.Sort)
            .ToListAsync(cancellationToken);
    }
}