using System.Xml.Linq;

namespace CurrencyUpdaterService.Infrastructure.External;

public interface ICurrencyApiClient
{
    Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync(CancellationToken ct);
}

public class CurrencyApiClient : ICurrencyApiClient
{
    private readonly HttpClient _http;

    public CurrencyApiClient(HttpClient http) => _http = http;
    
    public async Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync(CancellationToken ct)
    {
        var url = "http://www.cbr.ru/scripts/XML_daily.asp";
        var xml = await _http.GetStringAsync(url, ct);
        var doc = XDocument.Parse(xml);

        return new List<CurrencyDto>();

    }
}