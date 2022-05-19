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

    public async Task<List<EventListDto>> GetEventList(Guid? tagId = null)
    {
        return await GetEvent().Where(e => 
                tagId == null || e.EventTags.Any(t => t.TagId.ToString() == tagId.ToString()
                ))
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
                    UserName = p.User.UserName,
                    AvatarUrl = p.User.AvatarUrl
                }),

                Tags = e.EventTags.Select(p => new TagDto
                {
                    Name = p.Tag.Name,
                    Id = p.Tag.Id
                }),
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();
    }
    
    public async Task<Event> CreateEvent(EventCreateDto body)
    {
        var newEvent = _context.Events.Add(new Event
        {
            Name = body.Name,
            Location = body.Location,
            ImageUrl = body.ImageUrl,
            OwnerId = body.OwnerId,
        });
        
        await _context.SaveChangesAsync();

        return newEvent.Entity;
    }

    public async Task<Event> EditEvent(Guid id, EventCreateDto body)
    {
        var targetEvent = await _context.Events.SingleOrDefaultAsync(tEvent => tEvent.Id == id);

        targetEvent.Name = body.Name;
        targetEvent.Location = body.Location;
        targetEvent.ImageUrl = body.ImageUrl;
        targetEvent.OwnerId = body.OwnerId;

        await _context.SaveChangesAsync();

        return targetEvent;
    }

    public async void LinkTags(Guid id, IEnumerable<Guid> tags)
    {
        foreach (var tag in tags)
        {
            _context.EventTags.Add(new EventTag {
                EventId = id,
                TagId = tag
            });
        }
        
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

            Users = e.Participants.Select(p => new UserEventDto
            {
                UserName = p.User.UserName,
                AvatarUrl = p.User.AvatarUrl
            }).ToList(),
            Tags = e.EventTags.Select(p => new TagDto
            {
                Name = p.Tag.Name,
                Id = p.Tag.Id
            }).ToList(),

            CreatedAt = e.CreatedAt
        }).FirstOrDefaultAsync(e => e.Id == id);
    }

    public IIncludableQueryable<Event, Tag> GetEvent()
    {
        return _context.Events.Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .Include(e => e.EventTags)
            .ThenInclude(e => e.Tag);
    }

    public async Task<EventDto?> JoinEvent(Guid eventId, User user)
    {
        var participant = new Participant
        {
            UserId = user.Id,
            EventId = eventId
        };
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return await GetEvent(eventId);
    }

    public async Task<MessageResponse> LeaveEvent(Guid eventId, User user)
    {
        var participant =
            await _context.Participants.FirstOrDefaultAsync(p => p.UserId == user.Id && p.EventId == eventId);
        if (participant == null)
            return new MessageResponse
            {
                Success = false,
                Message = "You are not a participant of this event"
            };

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();

        return new MessageResponse
        {
            Success = true,
            Message = "You have left the event"
        };
    }
}