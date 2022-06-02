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
public class EventController : ControllerBase
{
    private readonly Context _context;
    private readonly EventManager _eventManager;
    private readonly UserManager<User> _userManager;

    public EventController(Context context, EventManager eventManager, UserManager<User> userManager)
    {
        _context = context;
        _eventManager = eventManager;
        _userManager = userManager;
    }

    // GET: api/event
    [HttpGet]
    public async Task<List<EventListDto>> Index([FromQuery] string? tagId)
    {
        return await _eventManager.GetEventList(tagId);
    }
    
    // GET: api/event/user/5
    [HttpGet("user/{ownerId:int}")]
    public async Task<List<EventListDto>> EventByOwner(int ownerId)
    {
        return await _eventManager.GetEventListByOwner(ownerId);
    }

    // GET: api/event/5
    [HttpGet("{id:int}")]
    public async Task<EventDto?> Get(int id)
    {
        return await _eventManager.GetEvent(id);
    }

    // POST: api/event/5/join
    [HttpPatch("{id:int}/join")]
    [Authorize]
    public async Task<EventDto?> Join(int id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        return await _eventManager.JoinEvent(id, user);
    }

    // POST: api/event/5/join
    [HttpDelete("{id:int}/leave")]
    [Authorize]
    public async Task<EventDto?> Leave(int id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        return await _eventManager.LeaveEvent(id, user);
    }
    
    // { nane: "fewjewfwfe", tags: ["idddd", "ididddd"] }

    // POST: api/event
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Event>> Post([FromBody] EventCreateDto value)
    {
        var owner = await HttpContext.User.GetUser(_userManager);
        
        if (owner == null)
            throw new Exception("User not found");

        if (owner.IsPremium == false)
        {
            var numberOfOwnerEvent = await _context.Events.CountAsync(e => e.OwnerId == owner.Id);
            if (numberOfOwnerEvent >= 2)
            {
                return new BadRequestObjectResult(new MessageResponse
                {
                    Success = false,
                    Message = "You can only create 2 events, upgrade to premium to create more or delete current event."
                });
            }
        }
        
        var newEvent = await _eventManager.CreateEvent(value, owner);
        return newEvent;
    }

    // PUT: api/Event/5
    [HttpPut("{id:int}")]
    public async Task<Event> Put(int id, [FromBody] EventCreateDto value)
    {
        var targetEvent = await _eventManager.EditEvent(id, value);
        return targetEvent;
    }

    // DELETE: api/Event/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var userId = HttpContext.User.GetId();
        var appEvent = await _context.Events.Include(e => e.Tags).SingleOrDefaultAsync(e => e.Id == id);

        if (appEvent.OwnerId != userId)
            return new UnauthorizedObjectResult(
                new MessageResponse
                {
                    Success = false,
                    Message = "You are not the owner of this event"
                });
        
        appEvent.Tags.Clear();
        _context.Events.Remove(appEvent);

        await _context.SaveChangesAsync();
        
        return new MessageResponse() { Message = I18n.Get("event_deleted") };
    }
}