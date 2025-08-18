using CurrencyService.Domain;
using CurrencyService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Infrastructure.Persistence;

/// <summary>
/// Контекст БД валют
/// </summary>
public class CurrencyDbContext : DbContext
{
    /// <summary>
    /// Контекст БД валют
    /// </summary>
    /// <param name="options">Опции</param>
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Представляет таблицу с курсами валют
    /// </summary>
    public DbSet<Currency> Currencies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rate).HasColumnType("decimal(18,10)");
        });
    }
}