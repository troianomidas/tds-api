using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;

namespace WebApi.Services.Reviews;

public record CreateReviewRequest : IRequest<int>
{
    public int StoreId { get; set; }
    public int OrderId { get; set; }
    public string? Comment { get; set; }
    public List<int> ReviewSegmentIds { get; set; }
}

public class CreateReviewRequestHandler : IRequestHandler<CreateReviewRequest, int>
{
    private readonly AppDbContext _context;

    public CreateReviewRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            StoreId = request.StoreId,
            OrderId = request.OrderId
        };

        foreach (var segmentsId in request.ReviewSegmentIds)
        {
            review.ReviewSegments.Add(new ReviewSegment(segmentsId));
        }

        if (request.Comment != null)
            review.Comment = request.Comment;

        _context.Reviews.Add(review);
        
        await _context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}