using hangnow_back.Authentications;
using hangnow_back.DataTransferObject;
using hangnow_back.Manager;
using hangnow_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hangnow_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly Context _context;
    private readonly EventManager _eventManager;
    private readonly UserManager<User> _userManager;

    public UserController(Context context, EventManager eventManager, UserManager<User> userManager)
    {
        _context = context;
        _eventManager = eventManager;
        _userManager = userManager;
    }
    
    // set user premium true
    [HttpPut("premium/{id}")]
    public async Task<IActionResult> SetPremium(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        
        // if is premium set false else true if null set true
        if (user.IsPremium == null)
        {
            user.IsPremium = true;
        }
        else
        {
            user.IsPremium = !user.IsPremium;
        }
        await _userManager.UpdateAsync(user);
        return Ok();
    }
}