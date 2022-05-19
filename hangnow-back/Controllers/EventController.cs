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

    // POST: api/event
    [Authorize]
    [HttpPost]
    public async Task<Event> Post([FromBody] Event value)
    {
        value.OwnerId = HttpContext.User.GetId();
        var newEvent = _context.Events.Add(value);
        await _context.SaveChangesAsync();
        return newEvent.Entity;
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