using hangnow_back.DataTransferObject;
using hangnow_back.Models;

namespace hangnow_back;

public class EventDto
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public UserEventDto? Owner { get; set; }

    public List<TagDto> Tags { get; set; }
    public List<UserEventDto>? Users { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class EventListDto
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    public int? OwnerId { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public IEnumerable<UserEventDto> Users { get; set; }
    public IEnumerable<TagDto> Tags { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class EventCreateDto
{
    public int? Id { get; set; }

    public string Name { get; set; }
    
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    
    public int? OwnerId { get; set; }

    //public List<UserEventDto>? Users { get; set; }
    public List<int> Tags { get; set; }
    
}