using hangnow_back.Models;

namespace Jwtest;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsPremium { get; set; }
    
    public static UserDto FromUser(AppUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            NormalizedUserName = user.NormalizedUserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsPremium = user.IsPremium
        };
    }
    
}
