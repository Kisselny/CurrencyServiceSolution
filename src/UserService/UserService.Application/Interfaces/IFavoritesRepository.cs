namespace UserService.Application.Interfaces;

/// <summary>
/// Определяет методы для управления избранными записями пользователей в системе
/// </summary>
public interface IFavoritesRepository
{
    /// <summary>
    /// Проверяет существование избранной записи для указанного пользователя и кода валюты в системе
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя</param>
    /// <param name="code">Код валюты для проверки в избранном пользователя</param>
    /// <param name="ct">Токен отмены для наблюдения за завершением задачи</param>
    /// <returns>True, если избранная запись существует, в противном случае false</returns>
    Task<bool> ExistsAsync(int userId, string code, CancellationToken ct = default);

    /// <summary>
    /// Добавляет валюту в избранное пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="code">Код валюты в системе</param>
    /// <param name="ct">Токен отмены</param>
    Task AddAsync(int userId, string code, CancellationToken ct = default);
    
    /// <summary>
    /// Польностью очищает список избранных валют пользователя
    /// </summary>
    /// <param name="userId">Идентификатор валюты</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Количество удаленных элементов</returns>
    Task<int> ClearAsync(int userId, CancellationToken ct = default);


    /// <summary>
    /// Возвращает список имен валют, находящихся в избранном у указанного пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список имен валют в избранном пользователя</returns>
    Task<IReadOnlyList<string>> InternalGetByUserAsync(int userId, CancellationToken ct = default);
}