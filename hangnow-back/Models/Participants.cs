using Microsoft.Build.Framework;

namespace hangnow_back.Models;

public class Participants
{
    public Guid Id { get; set; }

    [Required]
    public Guid EvenementId { get; set; }
    public Event Event { get; set; }
    
    [Required]
    public Guid UserId { get; set;  }
    public User User { get; set;  }
}