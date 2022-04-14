using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hangnow_back.Authentications;
using hangnow_back.Models;
using Jwtest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace hangnow_back.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppSettings _jwtConfig;
    private readonly UserManager<AppUser> _userManager;
    private readonly Context _context;

    public AuthController(UserManager<AppUser> userManager, IOptionsMonitor<AppSettings> optionsMonitor, Context context)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
        _context = context;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest user)
    {
        var newUser = new AppUser {Email = user.Email, UserName = user.Name};
        var isCreated = await _userManager.CreateAsync(newUser, user.Password);

        if (!isCreated.Succeeded)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description).ToList())
            });
        
        // 
        var roles = await _userManager.GetRolesAsync(newUser);
        
        var jwtToken = GenerateJwtToken(newUser, roles);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        
        if (existingUser == null)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = "Invalid authentication request"
            });

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isCorrect)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = "Invalid authentication request"
            });
        
        var roles = await _userManager.GetRolesAsync(existingUser);

        var jwtToken = GenerateJwtToken(existingUser, roles);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    private string GenerateJwtToken(AppUser user, IEnumerable<string>? roles)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        if (roles != null)
        {
            foreach (var role in roles)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe([FromQuery(Name = "isPayload")] string isPayload)
    {
        var user = HttpContext.User.GetUser(_userManager);

        return Ok(user);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _context.Users.FirstOrDefault(e => e.Id == id);
        return Ok(user);
    }
    
    // controller to add admin to a role
    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/{role}")]
    public IActionResult AddToRole(Guid id, string role)
    {
        var user = _context.Users.FirstOrDefault(e => e.Id == id);
        
        if (user == null)
            return BadRequest(new {message = "User not found"});
        
        var result = _userManager.AddToRoleAsync(user, role).Result;
        return Ok(result);
    }
    
}