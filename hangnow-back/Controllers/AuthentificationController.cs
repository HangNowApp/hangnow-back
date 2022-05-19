using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hangnow_back.Authentications;
using hangnow_back.DataTransferObject;
using hangnow_back.Models;
using Jwtest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace hangnow_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Context _context;
    private readonly AppSettings _jwtConfig;
    private readonly UserManager<User> _userManager;

    public AuthController(UserManager<User> userManager, IOptionsMonitor<AppSettings> optionsMonitor,
        Context context)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest user)
    {
        var newUser = new User {Email = user.Email, UserName = user.UserName};
        var isCreated = await _userManager.CreateAsync(newUser, user.Password);

        if (!isCreated.Succeeded)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description).ToList())
            });

        var roles = await _userManager.GetRolesAsync(newUser);

        var jwtToken = GenerateJwtToken(newUser, roles);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = I18n.Get("user_not_found")
            });

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isCorrect)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = I18n.Get("invalid_auth_request")
            });

        var roles = await _userManager.GetRolesAsync(existingUser);

        var jwtToken = GenerateJwtToken(existingUser, roles);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateRequest)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = I18n.Get("user_not_found")
            });

        user.UserName = updateRequest.UserName;
        user.Email = updateRequest.Email;
        user.AvatarUrl = updateRequest.AvatarUrl;

        await _context.SaveChangesAsync();

        return Ok(new MessageResponse
        {
            Success = true,
            Message = I18n.Get("user_updated")
        });
    }

    [Authorize]
    [HttpPatch("change_password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = I18n.Get("user_not_found")
            });

        var isCorrect = await _userManager.CheckPasswordAsync(user, changePasswordRequest.OldPassword);

        if (!isCorrect)
            return BadRequest(new RegistrationResponse
                {
                    Result = false,
                    Message = I18n.Get("invalid_old_password")
                }
            );

        var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword,
            changePasswordRequest.NewPassword);

        if (!result.Succeeded)
            
            return BadRequest(new MessageResponse
            {
                Success = false,
                Message = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description).ToList())
            });

        return Ok(new MessageResponse
        {
            Success = true,
            Message = I18n.Get("password_changed")
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        var jwtToken = GenerateJwtToken(user, roles);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await HttpContext.User.GetUser(_userManager);

        return Ok(user);
    }

    [Authorize(Policy = "Admin")]
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var user = _context.Users.FirstOrDefault(e => e.Id == id);

        if (user == null)
            return NotFound();

        return Ok(UserDto.FromUser(user));
    }

    // controller to add admin to a role
    [Authorize(Policy = "Admin")]
    [HttpPost("{id:guid}/{role}")]
    public IActionResult AddToRole(Guid id, string role)
    {
        var user = _context.Users.FirstOrDefault(e => e.Id == id);

        if (user == null)
            return BadRequest(new {message = I18n.Get("user_not_found")});

        var result = _userManager.AddToRoleAsync(user, role).Result;
        return Ok(result);
    }

    private string GenerateJwtToken(User user, IEnumerable<string>? roles)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        if (roles != null)
            foreach (var role in roles)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
}