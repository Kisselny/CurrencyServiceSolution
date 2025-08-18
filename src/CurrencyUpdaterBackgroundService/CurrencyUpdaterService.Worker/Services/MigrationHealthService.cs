namespace CurrencyUpdaterService.Worker.Services;

/// <inheritdoc />
public class MigrationHealthService : IMigrationHealthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// Представляет сервис для мониторинга готовности миграций базы данных
    /// </summary>
    /// <param name="httpClientFactory">HTTP-клиент</param>
    /// <param name="configuration">Конфигурация</param>
    public MigrationHealthService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    /// <inheritdoc />
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