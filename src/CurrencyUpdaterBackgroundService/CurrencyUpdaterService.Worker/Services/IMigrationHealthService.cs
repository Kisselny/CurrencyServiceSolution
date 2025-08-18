namespace CurrencyUpdaterService.Worker.Services;

/// Представляет сервис для мониторинга готовности миграций базы данных
public interface IMigrationHealthService
{
    /// Проверяет готовность миграций базы данных
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <return>True если миграции были применены, false в противном случае</return>
    Task<bool> IsMigrationReadyAsync(CancellationToken cancellationToken);
}