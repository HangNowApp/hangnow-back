using Microsoft.AspNetCore.Identity;

namespace hangnow_back.Authentications;

public static class SeedDataApplicationRoles
{
    public static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in new string[2] { "Admin", "User" })
        {
            var result =  roleManager.RoleExistsAsync(role).Result;
            if (!result)
            { 
                roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}