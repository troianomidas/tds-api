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

public record GetProductExtraByIdRequest : IRequest<ProductExtra?>
{
    public int StoreId { get; init; }
    public int Id { get; init; }
}

public class GetProductExtraByIdRequestHandler : IRequestHandler<GetProductExtraByIdRequest, ProductExtra?>
{
    private readonly AppDbContext _context;

    public GetProductExtraByIdRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductExtra?> Handle(GetProductExtraByIdRequest request,
        CancellationToken cancellationToken)
    {
        return await _context.ProductExtras.Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .Include(x => x.Items)
            .FirstOrDefaultAsync(cancellationToken);
    }
}