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
    public async Task<List<EventListDto>> Index([FromQuery] string? tagId) // add params tagId: Guid
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
        return newEvent;
    }

    // PUT: api/Event/5
    [HttpPut("{id}")]
    public async Task<Event> Put(Guid id, [FromBody] EventCreateDto value)
    {
        var targetEvent = await _eventManager.EditEvent(id, value);

        var relatedTags = await _context.EventTags.Where(tEvent => tEvent.EventId == id).ToListAsync();
        var newTags = value.Tags.Except(relatedTags.Select(tEvent => tEvent.TagId));
        var oldTags = relatedTags.Select(tEvent => tEvent.TagId).Except(value.Tags);
        
        foreach (var tag in oldTags)
        {
            _context.EventTags.Remove(relatedTags.Single(tEvent => tEvent.TagId == tag));
        }
        
        _eventManager.LinkTags(id, newTags);

        await _context.SaveChangesAsync();

        return targetEvent;
    }

    // DELETE: api/Event/5
    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        // Delete event and all related tags
        var relatedTags = _context.EventTags.Where(tEvent => tEvent.EventId == id);
        _context.EventTags.RemoveRange(relatedTags);
        _context.Events.Remove(new Event { Id = id });
        _context.SaveChanges();
    }
}