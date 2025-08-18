using CurrencyUpdaterService.Application;
using CurrencyUpdaterService.Infrastructure.External;
using CurrencyUpdaterService.Infrastructure.Persistence;
using CurrencyUpdaterService.Worker.Services;

namespace CurrencyUpdaterService.Worker
{
    /// <summary>
    /// Фоновый сервис, отвечающий за обновление курсов валют
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CustomServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Фоновый сервис, отвечающий за обновление курсов валют
        /// </summary>
        public Worker(ILogger<Worker> logger, CustomServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Периодически обращается во внеший API за новыми данными о валютах
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var timePeriod = TimeSpan.FromMinutes(5);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                
                await _scopeFactory.RunInScopeAsync(async provider =>
                {
                    var migrationHealthService = provider.GetRequiredService<IMigrationHealthService>();
                    
                    if (await migrationHealthService.IsMigrationReadyAsync(cancellationToken))
                    {
                        var apiClient = provider.GetRequiredService<ICurrencyApiClient>();
                        var currencies = await apiClient.FetchCurrenciesAsync(cancellationToken);
                        
                        var updateService = provider.GetRequiredService<ICurrencyUpdateService>();
                        await updateService.UpsertCurrenciesAsync(currencies);
                        _logger.LogInformation("Миграции применены. Данные запрошены из внешнего API и сохранены в базу данных. Следующее обновление через: {time} минут.", timePeriod.Minutes);
                    }
                    else
                    {
                        timePeriod = TimeSpan.FromMinutes(1);
                        _logger.LogWarning("Миграции не применены или сервис миграций не ответил. Данные не были запрошены из внешнего API и  не сохранены в базу данных. Следующая попытка через: {time} минут.", timePeriod.Minutes);
                    }
                });
                
                await Task.Delay(timePeriod, cancellationToken);
            }
        }
    }
}
