using hangnow_back.Authentications;
using hangnow_back.DataTransferObject;
using hangnow_back.Models;
using Microsoft.AspNetCore.Mvc;
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
        return await GetEventListDtos().Where(e => 
                tagId == null || e.Tags.Any(t => t.Id.ToString() == tagId)
                )
            .ToListAsync();
    }

    public async Task<List<EventListDto>> GetEventListByOwner(int ownerId)
    {
        return await GetEventListDtos()
            .Where(e => e.OwnerId == ownerId)
            .ToListAsync();

    }
    
    public async Task<Event> CreateEvent(EventCreateDto body, User owner)
    {
        var tags = await _context.Tags.Where(e => body.Tags.Contains(e.Id)).ToListAsync();

        var newEvent = new Event
        {
            Name = body.Name,
            Location = body.Location,
            Description = body.Description,
            ImageUrl = body.ImageUrl,
            OwnerId = body.OwnerId,
            Tags = tags,
            Users = new List<User> { owner }
        };

        var entity = await _context.Events.AddAsync(newEvent);

        await _context.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task<Event> EditEvent(int id, EventCreateDto body)
    {
        var appEvent = await _context.Events.Include(e => e.Tags).SingleOrDefaultAsync(e => e.Id == id);
        var tags = _context.Tags.Where(e => body.Tags.Contains(e.Id)).ToList();
        
        appEvent.Name = body.Name;
        appEvent.Description = body.Description;
        appEvent.Location = body.Location;
        appEvent.ImageUrl = body.ImageUrl;
        appEvent.OwnerId = body.OwnerId;
        appEvent.Tags = tags;

        await _context.SaveChangesAsync();

        return appEvent;
    }
    
    public async Task<EventDto?> GetEvent(int id)
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
            Users = e.Users.Select(p => new UserEventDto
            {
                Id = p.Id,
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
            .Include(e => e.Users)
            .Include(e => e.Tags);
    }

    public IQueryable<EventListDto> GetEventListDtos()
    {
        return GetEvent().Select(e =>
            new EventListDto
            {
                Id = e.Id,

                Name = e.Name,
                Location = e.Location,
                ImageUrl = e.ImageUrl,
                OwnerId = e.OwnerId,

                StartDate = e.StartDate,
                EndDate = e.EndDate,

                Users = e.Users.Select(p => new UserEventDto
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    AvatarUrl = p.AvatarUrl
                }),

                Tags = e.Tags.Select(p => new TagDto
                {
                    Name = p.Name,
                    Id = p.Id
                }),
                CreatedAt = e.CreatedAt
            });
    }

    public async Task<EventDto?> JoinEvent(int eventId, User user)
    {
        var appEvent = await _context.Events
            .Include(e => e.Users)
            .SingleOrDefaultAsync(e => e.Id.ToString() == eventId.ToString());
        
        if (appEvent == null)
            return null;
        
        appEvent.Users.Add(user);

        await _context.SaveChangesAsync();

        return await GetEvent(eventId);
    }

    public async Task<EventDto?> LeaveEvent(int eventId, User user)
    {
        var appEvent = await _context.Events
            .Include(e => e.Users)
            .SingleOrDefaultAsync(e => e.Id.ToString() == eventId.ToString());

        if (appEvent == null)
            return null;

        _context.Database.ExecuteSqlRaw("DELETE FROM \"EventUser\" WHERE \"UsersId\" = {0} AND \"EventsId\" = {1}", 
            user.Id.ToString().ToUpper(), eventId.ToString().ToUpper());

        await _context.SaveChangesAsync();

        return await GetEvent(eventId);
    }
}