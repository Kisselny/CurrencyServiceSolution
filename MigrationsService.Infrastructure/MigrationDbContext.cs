using Microsoft.EntityFrameworkCore;

using MigrationsService.Domain.Models;

namespace MigrationsService.Infrastructure;

public class MigrationDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }

    public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
            : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rate).HasColumnType("decimal(18,4)"); //TODO - 18 выглядит многовато
        });
    }
}