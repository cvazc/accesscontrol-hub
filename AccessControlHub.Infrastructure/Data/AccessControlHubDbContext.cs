using AccessControlHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessControlHub.Infrastructure.Data;

public class AccessControlHubDbContext : DbContext
{
    public AccessControlHubDbContext(DbContextOptions<AccessControlHubDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}