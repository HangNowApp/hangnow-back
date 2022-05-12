using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class EventTag
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public Guid EventId { get; set; }

    public Event Event { get; set; }

    [Required] public Guid TagId { get; set; }

    public Tag Tag { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}