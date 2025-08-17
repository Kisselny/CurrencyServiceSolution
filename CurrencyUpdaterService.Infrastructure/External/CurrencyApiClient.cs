using System.Globalization;
using System.Text;
using System.Xml.Linq;
using CurrencyUpdaterService.Application;
using CurrencyUpdaterService.Domain.Models;

namespace CurrencyUpdaterService.Infrastructure.External;

/// Предоставляет методы для получения данных о валютах из внешнего API
public class CurrencyApiClient : ICurrencyApiClient
{
    private readonly HttpClient _http;

    /// Предоставляет методы для получения данных о валютах из внешнего API
    public CurrencyApiClient(HttpClient http) => _http = http;

    /// Асинхронно получает список валют и их курсов из внешнего API
    /// <returns>Список валют</returns>
    public async Task<List<Currency>> FetchCurrenciesAsync()
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
        
        var currencies = MapDataToEntity(doc);
        
        return currencies;
    }

    private static List<Currency> MapDataToEntity(XDocument doc)
    {
        var currencies = doc
            .Descendants("Valute")
            .Select(v => new Currency
            {
                Name = (string?)v.Element("Name") ?? string.Empty,
                Rate = decimal.TryParse(
                    ((string?)v.Element("VunitRate"))?.Replace(',', '.') ?? "0",
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