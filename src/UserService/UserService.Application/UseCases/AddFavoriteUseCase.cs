using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace UserService.Application.UseCases;

/// Обрабатывает добавление валюты в список избранных пользователя
public class AddFavoriteUseCase
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly ILogger<AddFavoriteUseCase> _logger;
    private static readonly int MaxLen = 30;

    /// Обрабатывает добавление валюты в список избранных пользователя
    public AddFavoriteUseCase(IFavoritesRepository favoritesRepository, ILogger<AddFavoriteUseCase> logger)
    {
        _favoritesRepository = favoritesRepository;
        _logger = logger;
    }

    /// <summary>
    /// Запускает выполнение команды добавления валюты в список избранных
    /// </summary>
    /// <param name="command">Команда с данными для добавления валюты в избранное</param>
    /// <param name="ct">Токен отмены</param>
    public async Task Handle(AddFavoriteCommand command, CancellationToken ct = default)
    {
        if (command.UserId <= 0)
        {
            _logger.LogError("UserId меньше или равен 0: {UserId}", command.UserId);
            throw new ArgumentException("UserId должен быть больше нуля.");
        }
        var code = Normalize(command.CurrencyCode);
        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogError("Валюта содержит пустое название. UserId: {UserId}", command.UserId);
            throw new ArgumentException("Валюта должна иметь название.");
        }
        if (code.Length > MaxLen)
        {
            _logger.LogError("Название слишком длинное. UserId: {UserId}", command.UserId);
            throw new ArgumentException("Название слишком длинное.");
        }

        if (!await _favoritesRepository.ExistsAsync(command.UserId, code, ct))
        {
            _logger.LogInformation("Валюта {Code} добавлена в избранное пользователя {UserId}", code, command.UserId);
            await _favoritesRepository.AddAsync(command.UserId, code, ct);
        }
        else
        {
            _logger.LogInformation("Валюта {Code} уже есть в избранном у пользователя {UserId}", code, command.UserId);
        }
    }

    private static string Normalize(string s) => s?.Trim().ToUpperInvariant();
}