using hangnow_back.Models;
using Microsoft.EntityFrameworkCore;

namespace hangnow_back.Manager;

public class TagManager
{
    private readonly Context _context;

    public TagManager(Context context)
    {
        _context = context;
    }
    
    public async Task<Tag?> GetTag(int id)
    {
        return await _context.Tags.FindAsync(id);
    }
    
    public async Task<Tag?> CreateTag(string name, int userId) 
    {
        // does exist tag with same name
        if (_context.Tags.Any(t => t.Name.ToLower() == name.ToLower()))
        {
            return null;
        }
        
        var tag = new Tag
        {
            Name = name,
            CreatorId = userId
        };
        var createdTag = await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return createdTag.Entity;
    }
}