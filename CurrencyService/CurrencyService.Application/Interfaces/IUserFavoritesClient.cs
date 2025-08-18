namespace CurrencyService.Application.Interfaces;

/// Предоставляет методы для получения избранных элементов пользователя
public interface IUserFavoritesClient
{
    /// Получает список избранных элементов для заданного пользователя
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="ct">Токен отмены</param>
    /// <return>Список кодов избранных элементов</return>
    Task<IReadOnlyList<string>> GetFavoritesAsync(int userId, CancellationToken ct = default);
}