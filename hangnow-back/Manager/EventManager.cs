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

    public async Task<List<EventListDto>> GetEventList()
    {
        return await GetEvent().Select(e =>
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
                }).ToList(),
                Tags = e.EventTags.Select(p => p.Tag).ToList(),

                CreatedAt = e.CreatedAt
            }).ToListAsync();
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
            Tags = e.EventTags.Select(p => p.Tag).ToList(),

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