using System.ComponentModel.DataAnnotations;

namespace hangnow_back.DataTransferObject;

public class UpdateUserRequest
{
    [Required] public string UserName { get; set; }

    [Required] public string Email { get; set; }

    [Required] public string AvatarUrl { get; set; }
}

public class ChangePasswordRequest
{
    [Required] public string OldPassword { get; set; }
    [Required] public string NewPassword { get; set; }
}