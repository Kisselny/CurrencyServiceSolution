using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases;

/// Обрабатывает добавление валюты в список избранных пользователя
public class AddFavoriteUseCase
{
    private readonly IFavoritesRepository _favoritesRepository;
    private static readonly int MaxLen = 30;

    /// Обрабатывает добавление валюты в список избранных пользователя
    public AddFavoriteUseCase(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
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
            throw new ArgumentException("UserId должен быть больше нуля.");
        }
        var code = Normalize(command.CurrencyCode);
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Валюта должна иметь название.");
        }
        if (code.Length > MaxLen)
        {
            throw new ArgumentException("Название слишком длинное.");
        }

        if (!await _favoritesRepository.ExistsAsync(command.UserId, code, ct))
        {
            await _favoritesRepository.AddAsync(command.UserId, code, ct);
        }
    }

    private static string Normalize(string s) => s?.Trim().ToUpperInvariant();
}