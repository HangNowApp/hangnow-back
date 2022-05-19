using hangnow_back.Authentications;
using hangnow_back.DataTransferObject;
using hangnow_back.Manager;
using hangnow_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<List<EventListDto>> Index([FromQuery] Guid? tagId) // add params tagId: Guid
    {
        return await _eventManager.GetEventList(tagId);
    }

    // GET: api/event/5
    [HttpGet("{id:guid}")]
    public async Task<EventDto?> Get(Guid id)
    {
        return await _eventManager.GetEvent(id);
    }

    // POST: api/event/5/join
    [HttpPost("{id:guid}/join")]
    [Authorize]
    public async Task<EventDto?> Join(Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        return await _eventManager.JoinEvent(id, user);
    }

    // POST: api/event/5/join
    [HttpDelete("{id:guid}/leave")]
    [Authorize]
    public async Task<MessageResponse?> Leave(Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        return await _eventManager.LeaveEvent(id, user);
    }
    
    // { nane: "fewjewfwfe", tags: ["idddd", "ididddd"] }

    // POST: api/event
    [Authorize]
    [HttpPost]
    public async Task<Event> Post([FromBody] EventCreateDto value)
    {
        // value.OwnerId = HttpContext.User.GetId();
        var newEvent = await _eventManager.CreateEvent(value);
        
        await _context.SaveChangesAsync();
        
        // TODO: Create eventcreatedto and link tags to our new event
        foreach (var tag in value.Tags)
        {
            _context.EventTags.Add(new EventTag {
                EventId = newEvent.Id,
                TagId = tag
            });
        }

        await _context.SaveChangesAsync();
        
        return newEvent;
    }

    // PUT: api/Event/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
        
    }

    // DELETE: api/Event/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}