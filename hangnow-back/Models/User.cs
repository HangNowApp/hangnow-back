using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePicture { get; set; }
    
    public bool IsPremium { get; set; } = false;
}