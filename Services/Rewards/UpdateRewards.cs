using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public record UpdateRewardRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
    public decimal PointsCost { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateRewardRequestHandler : IRequestHandler<UpdateRewardRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateRewardRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateRewardRequest request, CancellationToken cancellationToken)
    {
        var reward = new Reward
        {
            PointsCost = request.PointsCost,
            Status = request.Status,
            ImageUrl = request.ImageUrl
        };

        if (request.Description != null)
            reward.Description = request.Description;

        Reward? rewardDb = await _context.Rewards.Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (rewardDb == null)
            throw new KeyNotFoundException();
        
        rewardDb.PointsCost = reward.PointsCost;
        rewardDb.Status = reward.Status;
        rewardDb.Description = reward.Description;
        rewardDb.ImageUrl = reward.ImageUrl;
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}