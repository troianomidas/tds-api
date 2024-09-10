using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class Segment : BaseEntity
{
    public int Group { get; set; }
    public string GroupName { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }

    public Segment(int group, string groupName, string? name, string? value)
    {
        Group = group;
        GroupName = groupName;
        Name = name;
        Value = value;
    }

    public virtual ReviewSegment? ReviewSegment { get; set; }
}