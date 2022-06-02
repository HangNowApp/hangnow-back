using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class Tag
{
    [Key] public int Id { get; set; }

    [MaxLength(20)]
    [MinLength(2)]
    [Required]
    public string Name { get; set; }

    public int CreatorId { get; set; }
    public User Creator { get; set; }
    
    public List<Event> Events { get; set; }
    public List<User> Users { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}