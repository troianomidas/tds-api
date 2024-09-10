using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class ReviewSegment : BaseEntity
{
    public int ReviewId { get; set; }
    public int SegmentId { get; set; }
    
    public ReviewSegment(int segmentId)
    {
        SegmentId = segmentId;
    }
    
    public virtual Review? Review { get; set; }
    public virtual Segment? Segment { get; set; } 
}