using AutoMapper;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using WebApi.Services.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record ListStore : IRequest<List<Store>>
{
    public string? CityState { get; set; }
    // public bool BypassCache { get; set; }
    // public string CacheKey => $"ListStore-{CityState}";
    // public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(30);
}

public class ListStoreHandler : IRequestHandler<ListStore, List<Store>>
{
    private readonly AppDbContext _context;

    public ListStoreHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<List<Store>> Handle(ListStore request, CancellationToken cancellationToken)
    {
        IQueryable<Store> query = _context.Stores
            .Include(x => x.Address)
            .Include(x => x.OpeningHours)
            .Include(x => x.StoreDelivery)
            .Where(x => x.Status == StoreStatusConst.Active);

        if (!string.IsNullOrEmpty(request.CityState))
            query = query.Where(x => x.Address != null && x.Address.CityState == request.CityState);

        return await query.ToListAsync(cancellationToken);
    }
}

public class ListStoreResponse
{
    public string? Name { get; set; }
    public string? Category { get; set; }
}