using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;

namespace WebApi.Services.Rewards;

public record CreateRewardRequest : IRequest<int>
{
    public int StoreId { get; set; }
    public decimal PointsCost { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateRewardRequestHandler : IRequestHandler<CreateRewardRequest, int>
{
    private readonly AppDbContext _context;

    public CreateRewardRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRewardRequest request, CancellationToken cancellationToken)
    {
        var reward = new Reward
        {
            StoreId = request.StoreId,
            PointsCost = request.PointsCost,
            Description = request.Description,
            Status = request.Status,
            ImageUrl = request.ImageUrl
        };

        _context.Rewards.Add(reward);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return reward.Id;
    }
}