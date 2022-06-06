using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class Event
{
    [Key] public int Id { get; set; }

    [MinLength(2)]
    [MaxLength(50)]
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? OwnerId { get; set; }
    public User? Owner { get; set; }

    public List<User> Users { get; set; }
    public List<Tag> Tags { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}