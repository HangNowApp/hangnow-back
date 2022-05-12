using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace hangnow_back.Models;

public class Context : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<EventTags> EventTags { get; set; }
    public DbSet<UserTags> UserTags { get; set; }
    public DbSet<Participants> Participants { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}