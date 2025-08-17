using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Application;

/// Представляет клиент для взаимодействия с внешним API валют
public interface ICurrencyApiClient
{
    /// Получает список валют с их обменными курсами из внешнего API
    /// <returns>Список валют с их обменными курсами</returns>
    Task<List<Currency>> FetchCurrenciesAsync();
}