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
public class TagController : ControllerBase
{
    private readonly Context _context;
    private readonly TagManager _tagManager;
    private readonly UserManager<User> _userManager;

    public TagController(Context context, TagManager tagManager, UserManager<User> userManager)
    {
        _context = context;
        _tagManager = tagManager;
        _userManager = userManager;
    }

    // GET: api/Tag
    [HttpGet]
    public async Task<List<Tag>> Get()
    {
        return await _context.Tags.ToListAsync();
    }

    // GET: api/Tag/5
    [HttpGet("{id:int}", Name = "Get")]
    public async Task<Tag?> Get(int id)
    {
        return await _tagManager.GetTag(id);
    }

    // POST: api/Tag
    [Authorize]
    [HttpPost]
    public async Task<Tag?> Post([FromBody] CreateTagDto value)
    {
        var tag = await _tagManager.CreateTag(value.Name, HttpContext.User.GetId());
        return tag;
    }

    // PUT: api/Tag/5
    // [Authorize]
    // [HttpPut("{id:int}")]
    // public void Put(int id, [FromBody] string value)
    // {
    // }

    // DELETE: api/Tag/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}