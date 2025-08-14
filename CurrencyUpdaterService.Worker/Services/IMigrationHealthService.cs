namespace CurrencyUpdaterService.Worker.Services;

public interface IMigrationHealthService
{
    Task<bool> IsMigrationReadyAsync(CancellationToken cancellationToken);
}