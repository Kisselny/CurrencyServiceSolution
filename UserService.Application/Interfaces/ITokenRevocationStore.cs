namespace UserService.Application.Interfaces;

/// Интерфейс для управления отзывом токенов
public interface ITokenRevocationStore
{
    /// Отзывает токен по идентификатору 
    /// <param name="jti">Идентификатор токена</param>
    /// <param name="expiresAt">Время истечения срока действия токена</param>
    /// <param name="userId">ID пользователя, связанного с токеном</param>
    /// <param name="ct">Токен отмены</param>
    Task RevokeAsync(string jti, DateTime expiresAt, int userId, CancellationToken ct = default);

    /// Проверяет, был ли токен отозван
    /// <param name="jti">Идентификатор токена</param>
    /// <param name="ct">Токен отмены</param>
    /// <return>True, если токен отозван, иначе False</return>
    Task<bool> IsRevokedAsync(string jti, CancellationToken ct = default);
}