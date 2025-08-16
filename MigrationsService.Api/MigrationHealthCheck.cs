using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MigrationsService.Infrastructure;

namespace MigrationsService.Api;

public class MigrationHealthCheck : IHealthCheck

{
    private readonly MigrationDbContext _dbContext;

    public MigrationHealthCheck(MigrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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