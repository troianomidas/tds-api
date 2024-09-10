using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Stores;
using WebApi.Services.Common.Mappings;

namespace WebApi.Services.OpeningHours;

public record GetOpeningHourRequest : IRequest<List<OpeningHour>>
{
    public int StoreId { get; set; }
}

public class GetOpeningHourRequestHandler : IRequestHandler<GetOpeningHourRequest, List<OpeningHour>>
{
    private readonly AppDbContext _context;

    public GetOpeningHourRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpeningHour>> Handle(GetOpeningHourRequest request, CancellationToken cancellationToken)
    {
        return await _context.OpeningHours.Where(x => x.StoreId == request.StoreId).ToListAsync(cancellationToken);
    }
}