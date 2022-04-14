using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Models;

public class User : IdentityUser
{
    public string CustomTag { get; set; }
}