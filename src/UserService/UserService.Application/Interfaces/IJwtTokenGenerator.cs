namespace UserService.Application.Interfaces;

/// <summary>
/// Предоставляет функциональность для генерации JWT токенов
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Генерирует JWT токен для указанного пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userName">Имя пользователя</param>
    /// <returns>Строка, содержащая сгенерированный JWT токен</returns>
    string GenerateToken(int userId, string userName);
}