using hangnow_back.DataTransferObject;
using hangnow_back.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace hangnow_back.Manager;

public class EventManager
{
    private readonly Context _context;

    public EventManager(Context context)
    {
        _context = context;
    }

    public async Task<List<EventListDto>> GetEventList(string? tagId = null)
    {
        return await GetEvent().Where(e => 
                tagId == null || e.Tags.Any(t => t.Id.ToString() == tagId)
                )
            .Select(e =>
            new EventListDto
            {
                Id = e.Id,
        
                Name = e.Name,
                Location = e.Location,
                ImageUrl = e.ImageUrl,
        
                StartDate = e.StartDate,
                EndDate = e.EndDate,
        
                Users = e.Participants.Select(p => new UserEventDto
                {
                    UserName = p.UserName,
                    AvatarUrl = p.AvatarUrl
                }),
        
                Tags = e.Tags.Select(p => new TagDto
                {
                    Name = p.Name,
                    Id = p.Id
                }),
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<Event> CreateEvent(EventCreateDto body)
    {
        var tags = _context.Tags.Where(e => body.Tags.Contains(e.Id.ToString())).ToList();

        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = body.Name,
            Location = body.Location,
            ImageUrl = body.ImageUrl,
            OwnerId = body.OwnerId,
            Tags = tags
        };

        await _context.Events.AddAsync(newEvent);

        await _context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<Event> EditEvent(Guid id, EventCreateDto body)
    {
        var targetEvent = await _context.Events.Include(e => e.Tags).SingleOrDefaultAsync(e => e.Id == id);
        var tags = _context.Tags.Where(e => body.Tags.Contains(e.Id.ToString())).ToList();
        
        targetEvent.Name = body.Name;
        targetEvent.Location = body.Location;
        targetEvent.ImageUrl = body.ImageUrl;
        targetEvent.OwnerId = body.OwnerId;
        targetEvent.Tags = tags;

        await _context.SaveChangesAsync();

        return targetEvent;
    }

    public async void LinkTags(Guid eventId, List<string> tags)
    {
        var dbTags = _context.Tags.Where(e => tags.Contains(e.Id.ToString())).ToList();
        var dbEvent = _context.Events.Find(eventId);
        
        dbEvent.Tags.AddRange(dbTags);
        
        await _context.SaveChangesAsync();
    }
    
    public async void DeleteTags(Guid eventId, List<string> tags)
    {
        var dbTags = _context.Tags.Where(e => tags.Contains(e.Id.ToString())).ToList();
        var dbEvent = _context.Events.Find(eventId);
        
        dbEvent.Tags.RemoveAll(e => dbTags.Contains(e));

        await _context.SaveChangesAsync();
    }
    public async Task<EventDto?> GetEvent(Guid id)
    {
        return await GetEvent().Select(e => new EventDto
        {
            Id = e.Id,
        
            Name = e.Name,
            Location = e.Location,
            ImageUrl = e.ImageUrl,
            Description = e.Description,
            Owner = e.Owner,
        
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            // Participants
            // Participants
            Users = e.Participants.Select(p => new UserEventDto
            {
                UserName = p.UserName,
                AvatarUrl = p.AvatarUrl
            }).ToList(),
            Tags = e.Tags.Select(p => new TagDto
            {
                Name = p.Name,
                Id = p.Id
            }).ToList(),
        
            CreatedAt = e.CreatedAt
        }).FirstOrDefaultAsync(e => e.Id == id);
    }

    public IIncludableQueryable<Event, List<Tag>> GetEvent()
    {
        return _context.Events
            .Include(e => e.Owner)
            .Include(e => e.Participants)
            .Include(e => e.Tags);
    }

    public async Task<EventDto?> JoinEvent(Guid eventId, User user)
    {
        var appEvent = await _context.Events.FindAsync(eventId);
        appEvent.Participants.Add(user);

        await _context.SaveChangesAsync();

        return await GetEvent(eventId);
    }

    public async Task<MessageResponse> LeaveEvent(Guid eventId, User user)
    {
        var customEvent = await _context.Events.FindAsync(eventId);
        
        if (customEvent == null)
            return new MessageResponse
            {
                Success = false,
                Message = "No event founded"
            };
        
        customEvent.Participants.Remove(user);
        
        await _context.SaveChangesAsync();

        return new MessageResponse
        {
            Success = true,
            Message = "You have left the event"
        };
    }
}