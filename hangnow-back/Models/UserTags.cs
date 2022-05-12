using Microsoft.Build.Framework;

namespace hangnow_back.Models;

public class UserTags
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid TagId { get; set; }
    
    public User User { get; set; }
    public Tag Tag { get; set; }
}