namespace CurrencyUpdaterService.Worker.Services;

public class MigrationHealthService : IMigrationHealthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    
    public MigrationHealthService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<bool> IsMigrationReadyAsync(CancellationToken cancellationToken)
    {
        var healthUrl = "https://localhost:7091/health"; //_configuration["MigrationService:HealthUrl"];
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync(healthUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content.Contains("Healthy");
        }
        catch
        {
            return false;
        }
    }
}