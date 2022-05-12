using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hangnow_back.Authentications;
using hangnow_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace hangnow_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly Context _context;
    private readonly UserManager<User> _userManager;

    public EventController(Context context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/event
    [HttpGet]
    public async Task<List<Event>> Get()
    {
        return await _context.Events.ToListAsync();
    }

    // GET: api/event/5
    [HttpGet("{id:guid}")]
    public async Task<Event?> Get(Guid id)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
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