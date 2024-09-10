using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public class ListStoreRewardsRequest : IRequest<List<Reward>>
{
    public int StoreId { get; init; }
    public int? Id { get; init; }
}

public class ListStoreRewardsRequestHandler : IRequestHandler<ListStoreRewardsRequest, List<Reward>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListStoreRewardsRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Reward>> Handle(ListStoreRewardsRequest request, CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        if (request.Id is not null)
            return await _context.Rewards.Where(x => x.StoreId == request.StoreId && x.Id == request.Id).ToListAsync(cancellationToken);
        
        return await _context.Rewards.Where(x => x.StoreId == request.StoreId).ToListAsync(cancellationToken);
    }
}