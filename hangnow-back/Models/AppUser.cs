using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class AppUser : IdentityUser<Guid>
{
    public bool IsPremium { get; set; } = false;
}