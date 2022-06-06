using hangnow_back.Models;

namespace hangnow_back;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public string PhoneNumber { get; set; }
    public IList<string> Roles { get; set; }

    public static UserDto FromUser(User user, IList<string> roles)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            NormalizedUserName = user.NormalizedUserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            Roles = roles
        };
    }
}

public class UserEventDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string? AvatarUrl { get; set; }
}