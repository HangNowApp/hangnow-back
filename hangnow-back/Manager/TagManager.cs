using hangnow_back.Models;

namespace hangnow_back.Manager;

public class TagManager
{
    private readonly Context _context;

    public TagManager(Context context)
    {
        _context = context;
    }
    
    public async Task<Tag?> GetTag(Guid id)
    {
        return await _context.Tags.FindAsync(id);
    }
    
    public async Task<Tag?> CreateTag(string name, Guid userId) 
    {
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