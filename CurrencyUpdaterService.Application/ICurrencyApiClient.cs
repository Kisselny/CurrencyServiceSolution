using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Application;

public interface ICurrencyApiClient
{
    Task<List<Currency>> FetchCurrenciesAsync();
}