using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.ExpiratedPoints;

public record ListExpiratedPointsRequest :  IRequest<List<ExpiratedPointsView>>
{
    
}

public class ListExpiratedPointsRequestHandler : IRequestHandler<ListExpiratedPointsRequest, List<ExpiratedPointsView>>
{
    private readonly AppDbContext _context;

    public ListExpiratedPointsRequestHandler(AppDbContext context) => _context = context;
    
    public async Task<List<ExpiratedPointsView>> Handle(ListExpiratedPointsRequest request,
        CancellationToken cancellationToken)
    {
        return await _context.ExpiratedPointsVw.ToListAsync(cancellationToken);
    }
}