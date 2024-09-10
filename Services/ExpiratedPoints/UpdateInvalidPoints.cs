using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.ExpiratedPoints;

public class UpdateInvalidPoints : IRequest<bool>
{
    public List<ExpiratedPointsView>? ListExpiratedPoints { get; set; }
}

public class UpdateInvalidPointsHandler : IRequestHandler<UpdateInvalidPoints, bool>
{
    private readonly AppDbContext _context;

    public UpdateInvalidPointsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateInvalidPoints request, CancellationToken cancellationToken)
    {
        if (request.ListExpiratedPoints == null)
            return false;

        foreach (var item in request.ListExpiratedPoints)
        {
            var resp = await _context.RewardsSystem
                .Where(x => x.Id == item.Id && x.StoreId == item.StoreId && x.CustomerId == item.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (resp == null)
                continue;

            resp.MovType = 3;
            resp.InvalidPoints = item.ExpiratedPoints * -1;
            resp.ExpirationTerm = item.ExpirationTerm;
            resp.ValidPoints = 0;
            
            await _context.SaveChangesAsync(cancellationToken);
        }

        return true;
    }
}