using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Common.Mappings;

namespace WebApi.Services.Categories;

public record GetCategoryRequest : IRequest<ProductCategory?>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
}

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, ProductCategory?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductCategory?> Handle(GetCategoryRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");
        
        if (request.Id < 1)
            throw new BadRequestException("Informe o código da categoria.");

        return await _context.Categories.Where(x => x.StoreId == request.StoreId && x.Status == 1 && x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}