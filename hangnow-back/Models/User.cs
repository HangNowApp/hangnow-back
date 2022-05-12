using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }

    public bool IsPremium { get; set; } = false;

    public ICollection<UserTag> Tags { get; set; }
    public ICollection<Participant> Events { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}