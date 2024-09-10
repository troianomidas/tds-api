using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public record ListRewardTransactionsRequest : IRequest<PaginatedList<ListRewardTransactionResponse>>
{
    public int StoreId { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 5;
}
    
public class ListRewardTransactionsRequestHandler : IRequestHandler<ListRewardTransactionsRequest, PaginatedList<ListRewardTransactionResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListRewardTransactionsRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ListRewardTransactionResponse>> Handle(ListRewardTransactionsRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        return await _context.RewardsSystem.Include(x => x.Reward)
            .Where(x => x.StoreId == request.StoreId && x.MovType == 2).OrderBy(x=>x.CreatedAt)
            .ProjectTo<ListRewardTransactionResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);
    }
}

public class ListRewardTransactionResponse : IMapFrom<RewardSystem>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int CustomerId { get; set; }
    public int? RewardId { get; set; }
    public decimal InvalidPoints { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public RewardResponse? Reward { get; set; }
}

public class RewardResponse : IMapFrom<Reward>
{
    public decimal PointsCost { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}