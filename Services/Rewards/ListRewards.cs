using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Rewards;

public class ListRewardsRequest : IRequest<List<Reward>>
{
    public int StoreId { get; init; }
    public int? Status { get; set; }
}

public class ListRewardsRequestHandler : IRequestHandler<ListRewardsRequest, List<Reward>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListRewardsRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Reward>> Handle(ListRewardsRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        IQueryable<Reward> query = _context.Rewards.Where(x => x.StoreId == request.StoreId);

        if (request.Status != null)
            query = query.Where(x => x.Status != 9);

        return await query.ToListAsync(cancellationToken);
    }
}