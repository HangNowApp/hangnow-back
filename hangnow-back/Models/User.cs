using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }

    public bool IsPremium { get; set; } = false;

    public List<Tag> Tags { get; set; }
    public List<Tag> CreatedTags { get; set; }
    public List<Event> OwnerEvents { get; set; }
    public List<Event> Events { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}