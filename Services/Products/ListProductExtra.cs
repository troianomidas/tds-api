using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;

namespace WebApi.Services.Products;

public record ListProductExtraRequest : IRequest<List<ProductExtra>>
{
    public int StoreId { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class ListProductExtraRequestHandler : IRequestHandler<ListProductExtraRequest, List<ProductExtra>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListProductExtraRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductExtra>> Handle(ListProductExtraRequest request,
        CancellationToken cancellationToken)
    {
        return await _context.ProductExtras.Where(x => x.StoreId == request.StoreId)
            .Include(x => x.Items)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}