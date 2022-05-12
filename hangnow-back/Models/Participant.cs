using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class Participant
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public Guid EventId { get; set; }

    public Event Event { get; set; }

    [Required] public Guid UserId { get; set; }

    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}