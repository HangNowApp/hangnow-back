using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace hangnow_back.Models;

public class Context : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Event>().HasOne(u => u.Owner)
            .WithMany(t => t.OwnerEvents)
            .HasForeignKey(u => u.OwnerId);
        modelBuilder.Entity<Event>().HasMany(e => e.Participants)
            .WithMany(e => e.Participations);

        modelBuilder.Entity<Tag>().HasOne(u => u.Creator)
            .WithMany(t => t.CreatedTags)
            .HasForeignKey(u => u.CreatorId);
        
        // modelBuilder.Entity<User>().HasMany(u => u.OwnerEvents)
        //     .WithOne(e => e.Owner)
        //     .HasForeignKey(u => u.OwnerId);
        // modelBuilder.Entity<User>().HasMany(u => u.CreatedTags)
        //     .WithOne(e => e.Creator)
        //     .HasForeignKey(u => u.CreatorId);

    }
}