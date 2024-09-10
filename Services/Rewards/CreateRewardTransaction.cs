using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public record CreateRewardTransactionRequest : IRequest<int>
{
    public int StoreId { get; set; }
    public int? CustomerId { get; set; }
    public int RewardId { get; set; }
}

public class CreateRewardTransactionRequestHandler : IRequestHandler<CreateRewardTransactionRequest, int>
{
    private readonly AppDbContext _context;

    public CreateRewardTransactionRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRewardTransactionRequest request, CancellationToken cancellationToken)
    {
        var rewardSystem = new RewardSystem
        {
            StoreId = request.StoreId,
            RewardId = request.RewardId,
            CustomerId = request.CustomerId
        };

        Reward? rewardDb = await _context.Rewards.Where(x => x.Id == request.RewardId && x.StoreId == request.StoreId
            && x.Status == 1)
            .FirstOrDefaultAsync(cancellationToken);

        if (rewardDb == null)
            throw new InvalidOperationException("Recompensa não encontrada!");
        
        CustomerBalancePointsView? balancePointsViewDb = await _context.CustomerBalancePointsVw
            .Where(x => x.StoreId == request.StoreId && x.CustomerId == request.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (balancePointsViewDb == null)
            throw new InvalidOperationException("");

        if (balancePointsViewDb.BalancePoints < rewardDb.PointsCost)
            throw new InvalidOperationException("Pontos Insuficientes!");
        
        rewardSystem.InvalidPoints = rewardDb.PointsCost * -1;
        rewardSystem.MovType = 2;

        _context.RewardsSystem.Add(rewardSystem);

        await _context.SaveChangesAsync(cancellationToken);

        return rewardSystem.Id;
     }
}