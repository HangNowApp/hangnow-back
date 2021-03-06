using System.ComponentModel.DataAnnotations;

namespace Jwtest;

public class RegistrationRequest
{
    [Required] public string UserName { get; set; }

    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}