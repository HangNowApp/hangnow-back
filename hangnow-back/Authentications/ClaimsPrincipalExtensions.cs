using System.Security.Claims;
using hangnow_back.Models;
using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Authentications;

public static class ClaimsPrincipalExtensions
{
    public static int GetId(this ClaimsPrincipal principal)
    {
        var id = principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

        return id == null ? 0 : int.Parse(id);
    }

    public static async Task<User?> GetUser(this ClaimsPrincipal principal, UserManager<User> userManager)
    {
        return await userManager.GetUserAsync(principal);
    }
}