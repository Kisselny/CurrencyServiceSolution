using Microsoft.Extensions.Logging;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases;

/// Обрабатывает удаление всех валют из списка избранных пользователя
public class ClearFavoritesUseCase
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly ILogger<ClearFavoritesUseCase> _logger;
    /// <summary>
    /// Обрабатывает удаление всех валют из списка избранных пользователя
    /// </summary>
    /// <param name="favoritesRepository">Репозиторий избранного</param>
    public ClearFavoritesUseCase(IFavoritesRepository favoritesRepository, ILogger<ClearFavoritesUseCase> logger)
    {
        _favoritesRepository = favoritesRepository;
        _logger = logger;
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
            _logger.LogError("UserId должен быть больше нуля");
            throw new ArgumentException("UserId должен быть больше нуля");
        }
        return await _favoritesRepository.ClearAsync(cmd.UserId, ct);
    }
}