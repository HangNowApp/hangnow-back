using Microsoft.Build.Framework;

namespace hangnow_back.Models;

public class EventTags
{
    [Required]
    public Guid EventId { get; set; }
    public Event Event { get; set; }

    [Required]
    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
}