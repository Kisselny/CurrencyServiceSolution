using UserService.Domain.Models;

namespace UserService.Application.Interfaces;

/// Интерфейс для работы с репозиторием пользователей
public interface IUserRepository
{
    /// Проверяет существование пользователя с указанным именем
    /// <param name="name">Имя пользователя для проверки</param>
    /// <return>True если пользователь существует, иначе false</return>
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);

    /// Возвращает пользователя с указанным именем
    /// <param name="name">Имя пользователя для поиска</param>
    /// <return>Пользователь с указанным именем или null, если не найден</return>
    Task<User> GetByNameAsync(string name, CancellationToken ct);

    /// Добавляет нового пользователя
    /// <param name="user">Пользователь для добавления</param>
    /// <param name="ct"></param>
    Task AddNewUserAsync(User user, CancellationToken ct);
}