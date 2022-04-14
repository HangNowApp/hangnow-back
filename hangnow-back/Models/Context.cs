using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace hangnow_back.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class Context: IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}