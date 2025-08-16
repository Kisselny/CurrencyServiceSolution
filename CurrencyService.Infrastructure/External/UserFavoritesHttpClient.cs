using System.Net.Http.Json;
using System.Text.Json;
using CurrencyService.Application.Interfaces;

namespace CurrencyService.Infrastructure.External;

public class UserFavoritesHttpClient : IUserFavoritesClient
{
    private readonly HttpClient _httpClient;
    private readonly UserServiceClientOptions _options;
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public UserFavoritesHttpClient(HttpClient httpClient, UserServiceClientOptions options)
    {
        _httpClient = httpClient;
        _options = options;
        
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new InvalidOperationException("BaseUrl не сконфигурирован.");
        _httpClient.BaseAddress = new Uri(_options.BaseUrl, UriKind.Absolute);        
    }

    public async Task<IReadOnlyList<string>> GetFavoritesAsync(int userId, CancellationToken ct = default)
    {
        // внутренний маршрут UserService (скрытый из Swagger)
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/internal/users/{userId}/favorites");

        using var response = await _httpClient.SendAsync(request, ct);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return Array.Empty<string>(); // у юзера нет favorites

        response.EnsureSuccessStatusCode();

        var list = await response.Content.ReadFromJsonAsync<List<string>>(_json, ct)
                   ?? new List<string>();
        
        return list
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToUpperInvariant())
            .Distinct()
            .ToList()
            .AsReadOnly();
    }
}