using CurrencyService.Domain;
using CurrencyService.Domain.Models;

namespace CurrencyService.Application.Interfaces;

/// <summary>
/// Репозиторий валют
/// </summary>
public interface ICurrencyRepository
{
    /// <summary>
    /// Получает валюты по кодам/названиям
    /// </summary>
    /// <param name="codes">Коды/названия валют</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Коллекция объектов валюты</returns>
    Task<IReadOnlyList<Currency>> GetByCodesAsync(
        IEnumerable<string> codes,
        CancellationToken ct = default);

    /// <summary>
    /// Получает все валюты
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Коллекция объектов валюты</returns>
    Task<IReadOnlyList<Currency>> GetAllAsync(
        CancellationToken ct = default);
}