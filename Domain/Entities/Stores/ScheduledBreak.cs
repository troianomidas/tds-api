using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Stores;

public class ScheduledBreak : BaseStoreEntity
{
    public ScheduledBreak()
    {
        
    }

    public ScheduledBreak(int storeId, string? title, DateTime startAt, DateTime endAt)
    {
        StoreId = storeId;
        Title = title;
        StartAt = startAt;
        EndAt = endAt;
        CreatedAt = DateTimeUtils.Now();
    }

    public string? Title { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}