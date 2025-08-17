using Microsoft.EntityFrameworkCore;
using MigrationsService.Domain.Models;
using MigrationsService.Infrastructure.Persistence.PersistentModels;

namespace MigrationsService.Infrastructure.Persistence;

/// <summary>
/// Контекст БД сервиса миграций 
/// </summary>
public class MigrationDbContext : DbContext
{
    /// <summary>
    /// Представляет таблицу валют
    /// </summary>
    public DbSet<Currency> Currencies { get; set; }
    /// <summary>
    /// Представляет таблицу пользователей
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Представляет таблицу избранных валют пользователей
    /// </summary>
    public DbSet<FavoriteRow> Favorites { get; set; }

    /// <summary>
    /// Контекст БД сервиса миграций 
    /// </summary>
    /// <param name="options">Опции</param>
    public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
            : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rate).HasColumnType("decimal(18,10)");
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("AppUsers");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();;

            entity.Property(u => u.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(22);

            entity.Property<string>("_password")
                .HasField("_password")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(64);

            entity.HasIndex(u => u.Name).IsUnique();
        });
        
        var fav = modelBuilder.Entity<FavoriteRow>();
        fav.ToTable("user_favorite_currency");
        fav.HasKey(x => new { x.UserId, CurrencyCode = x.CurrencyName });
        fav.Property(x => x.UserId).HasColumnName("user_id");
        fav.Property(x => x.CurrencyName).HasColumnName("currency_name")
            .HasMaxLength(30)
            .IsRequired();
        fav.HasIndex(x => x.UserId);
    }
}