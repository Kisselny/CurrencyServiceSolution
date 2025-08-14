using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Application;

public interface ICurrencyUpdateService
{
    Task UpsertCurrenciesAsync(IEnumerable<Currency> currencies);
}