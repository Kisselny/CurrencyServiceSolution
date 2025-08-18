using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases;

/// Обрабатывает удаление всех валют из списка избранных пользователя
public class ClearFavoritesUseCase
{
    private readonly IFavoritesRepository _favoritesRepository;
    /// <summary>
    /// Обрабатывает удаление всех валют из списка избранных пользователя
    /// </summary>
    /// <param name="favoritesRepository">Репозиторий избранного</param>
    public ClearFavoritesUseCase(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }

    /// <summary>
    /// Запускает обработку команды очистки списка избранных валют пользователя
    /// </summary>
    /// <param name="cmd">Команда с идентификатором пользователя</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Количество удаленных элементов</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если идентификатор пользователя некорректен</exception>
    public async Task<int> Handle(ClearFavoritesCommand cmd, CancellationToken ct = default)
    {
        if (cmd.UserId <= 0)
        {
            throw new ArgumentException("Invalid user id");
        }
        return await _favoritesRepository.ClearAsync(cmd.UserId, ct);
    }
}