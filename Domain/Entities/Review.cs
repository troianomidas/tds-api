using WebApi.Domain.Common;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities;

public class Review : BaseStoreEntity 
{
    public int OrderId { get; set; }
    public string? Comment { get; set; }
    public List<ReviewSegment> ReviewSegments { get; set; }
    
    public virtual Order? Order { get; set; }
    
    public Review()
    {
        ReviewSegments = new List<ReviewSegment>();
    }

    public Review(int orderId, string? comment, List<ReviewSegment> reviewSegments)
    {
        if (ReviewSegments is { Count: 0 }) 
            throw new InvalidOperationException(InputMsg.Required, new Exception("reviewSegmentsList"));

        OrderId = orderId;
        ReviewSegments = reviewSegments;
    }
}