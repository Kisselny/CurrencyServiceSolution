using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace CurrencyUpdaterService.Infrastructure.External;

public interface ICurrencyApiClient
{
    Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync();
}

public class CurrencyApiClient : ICurrencyApiClient
{
    private readonly HttpClient _http;

    public CurrencyApiClient(HttpClient http) => _http = http;
    
    public async Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync()
    {
        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var url = "http://www.cbr.ru/scripts/XML_daily.asp";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36 Edg/139.0.0.0");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var bytes = await response.Content.ReadAsByteArrayAsync();
        var encoding = Encoding.GetEncoding("windows-1251");
        var xml = encoding.GetString(bytes);
        var doc = XDocument.Parse(xml);
        
        var currencies = doc
            .Descendants("Valute")
            .Select(v => new CurrencyDto
            {
                Id = (string?)v.Attribute("ID") ?? string.Empty,
                CharCode = (string?)v.Element("CharCode") ?? string.Empty,
                Name = (string?)v.Element("Name") ?? string.Empty,
                Nominal = int.TryParse((string?)v.Element("Nominal"), out var n) ? n : 1,
                Value = decimal.TryParse(
                    ((string?)v.Element("Value"))?.Replace(',', '.') ?? "0",
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var d) 
                    ? d 
                    : 0m
            })
            .ToList();
        
        return currencies;
    }
    
    
}