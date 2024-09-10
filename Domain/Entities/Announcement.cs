using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class Announcement : BaseStoreEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
}