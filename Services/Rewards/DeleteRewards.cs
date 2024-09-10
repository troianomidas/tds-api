using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public record DeleteRewardRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
}

public class DeleteRewardRequestHandler : IRequestHandler<DeleteRewardRequest, bool>
{
    private readonly AppDbContext _context;

    public DeleteRewardRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteRewardRequest request, CancellationToken cancellationToken)
    {
        var rewardDb = 
            await _context.Rewards.Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (rewardDb == null)
            throw new KeyNotFoundException();

        _context.Rewards.Remove(rewardDb);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}