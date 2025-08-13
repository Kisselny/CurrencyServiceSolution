using CurrencyUpdaterService.Infrastructure.External;

namespace CurrencyUpdaterService.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICurrencyApiClient _apiClient;

        public Worker(ILogger<Worker> logger, ICurrencyApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await _apiClient.FetchCurrenciesAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
