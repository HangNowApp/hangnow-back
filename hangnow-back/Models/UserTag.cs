using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class UserTag
{
    public Guid Id { get; set; }

    [Required] public Guid UserId { get; set; }

    [Required] public Guid TagId { get; set; }

    public User User { get; set; }
    public Tag Tag { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}