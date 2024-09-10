using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;

namespace WebApi.Services.OpeningHours;

public record GetCountRequest : IRequest<int>
{
    public int StoreId { get; set; }
}

public class GetCountRequestHandler : IRequestHandler<GetCountRequest, int>
{
    private readonly AppDbContext _context;

    public GetCountRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetCountRequest request, CancellationToken cancellationToken)
    {
        return await _context.OpeningHours.Where(x => x.StoreId == request.StoreId).CountAsync(cancellationToken);
    }
}