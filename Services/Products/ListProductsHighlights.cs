using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Common.Mappings;

namespace WebApi.Services.Products;

public record ListProductsHighlights : IRequest<List<VwProductHighlights>>
{
}

public class ListProductsHighlightsHandler : IRequestHandler<ListProductsHighlights, List<VwProductHighlights>>
{
    private readonly AppDbContext _context;

    public ListProductsHighlightsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<VwProductHighlights>> Handle(ListProductsHighlights request, CancellationToken cancellationToken)
    {
        Console.WriteLine("bateu aqui");
        return await _context.VwProductHighlights.ToListAsync(cancellationToken);
    }
}