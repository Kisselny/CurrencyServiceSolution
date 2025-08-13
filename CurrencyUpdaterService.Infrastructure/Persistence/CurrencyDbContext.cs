using CurrencyUpdaterService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdaterService.Infrastructure.Persistence;

public class CurrencyDbContext : DbContext
{
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Currency> Currencies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rate).HasColumnType("decimal(18,4)");
        });
    }

}