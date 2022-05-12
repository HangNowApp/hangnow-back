using System.ComponentModel.DataAnnotations;

namespace hangnow_back.Models;

public class Tag
{
    public Guid Id { get; set; }
    
    [MaxLength(20)]
    [MinLength(2)]
    [Microsoft.Build.Framework.Required]
    public string Name { get; set; }
    
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
}