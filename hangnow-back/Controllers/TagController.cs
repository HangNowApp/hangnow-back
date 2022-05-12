using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hangnow_back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hangnow_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly Context _context;

    public TagController(Context context)
    {
        _context = context;
    }

    // GET: api/Tag
    [HttpGet]
    public async Task<List<Tag>> Get()
    {
        return await _context.Tags.ToListAsync();
    }

    // GET: api/Tag/5
    [HttpGet("{id}", Name = "Get")]
    public string Get(int id)
    {
        return null;
    }

    // POST: api/Tag
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT: api/Tag/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/Tag/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}