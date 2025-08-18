using CurrencyUpdaterService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdaterService.Infrastructure.Persistence;

/// Контекст базы данных для работы с сущностями Currency
public class CurrencyDbContext : DbContext
{
    /// Контекст базы данных для работы с сущностями Currency
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
        : base(options)
    {
    }

    /// Коллекция объектов типа Currency, представляющая таблицу Currencies в базе данных
    public DbSet<Currency> Currencies { get; set; } = null!;

    /// Настройка модели для сущности Currency
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