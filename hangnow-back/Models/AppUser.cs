using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class AppUser: IdentityUser<Guid>
{
    public AppUser(): base()
    {
    }

    public bool IsPremium { get; set; } = false;
}
