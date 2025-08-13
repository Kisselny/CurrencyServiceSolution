namespace CurrencyUpdaterService.Infrastructure.External;

public interface ICurrencyApiClient
{
    Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync();
}