using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;

namespace WebApi.Services.Reviews;

public record ListReviewsRequest : IRequest<PaginatedList<ListReviewResponse>>
{
    public int StoreId { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class ListReviewsHandler : IRequestHandler<ListReviewsRequest, PaginatedList<ListReviewResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListReviewsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ListReviewResponse>> Handle(ListReviewsRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        return await _context.Reviews
            .Include(x => x.ReviewSegments).ThenInclude(x => x.Segment)
            .Include(x => x.Order)
            .Where(x => x.StoreId == request.StoreId && x.CreatedAt >= request.From  && x.CreatedAt <= request.To).OrderBy(x => x.CreatedAt)
            .ProjectTo<ListReviewResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);
    }
}

public class ListReviewResponse : IMapFrom<Review>
{
    public int OrderId { get; set; }
    public string? Comment { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public int Group { get; set; }
    public string? GroupName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public OrderResponse? Order { get; set; }
    public List<ReviewSegment> ReviewSegments { get; set; }
}

public class OrderResponse : IMapFrom<Order>
{
    public int Number { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItem> Items { get; set; }
}
