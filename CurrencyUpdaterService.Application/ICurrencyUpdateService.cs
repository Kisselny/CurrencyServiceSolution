using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Application;

/// Предоставляет методы для обновления или вставки данных о валютах в базу данных
public interface ICurrencyUpdateService
{
    /// Обновляет или вставляет данные о валютах в базу данных
    /// <param name="currencies">Коллекция объектов Currency для обновления/вставки</param>
    Task UpsertCurrenciesAsync(IEnumerable<Currency> currencies);
}