namespace MigrationsService.Infrastructure.Persistence.PersistentModels;

/// <summary>
/// Представляет модель ассоциации пользователя и избранного
/// </summary>
public class FavoriteRow
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Идентификатор валюты
    /// </summary>
    public string CurrencyName { get; set; } = null!;
}