namespace UserService.Infrastructure.Persistence;

/// Представляет избранную валюту пользователя
public class FavoriteRow
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Код/название валюты
    /// </summary>
    public string CurrencyName { get; set; } = null!;
}