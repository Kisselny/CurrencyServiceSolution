using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MigrationsService.Infrastructure;

namespace MigrationsService.Api;

/// Представляет проверку статуса миграций базы данных
public class MigrationHealthCheck : IHealthCheck

{
    private readonly MigrationDbContext _dbContext;

    /// Представляет проверку статуса миграций базы данных
    public MigrationHealthCheck(MigrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// Выполняет проверку состояния миграций базы данных
    /// <param name="context">Контекст проверки состояния</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <return>Результат проверки состояния</return>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var appliedMigrations = await _dbContext.Database.GetAppliedMigrationsAsync(cancellationToken);

            if (appliedMigrations.Any())
                return HealthCheckResult.Healthy("Есть применённые миграции.");
            return HealthCheckResult.Unhealthy("Миграции не найдены.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Миграции не применялись (таблица истории миграций не найдена)", ex);
        }

    }

}