using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Infrastructure.External;

public interface ICurrencyApiClient
{
    Task<List<Currency>> FetchCurrenciesAsync();
}