using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Persistence;

/// <summary>
/// Контекст БД пользователей
/// </summary>
public class UserDbContext : DbContext
{
    /// <summary>
    /// Контекст БД пользователей
    /// </summary>
    /// <param name="options">Опции</param>
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    /// <summary>
    /// Представляет таблицу пользователей
    /// </summary>
    public DbSet<User> Users => Set<User>();
    /// <summary>
    /// Представляет таблицу кодов избранных валют пользователей
    /// </summary>
    public DbSet<FavoriteRow> Favorites => Set<FavoriteRow>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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