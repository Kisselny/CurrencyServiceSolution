using Microsoft.EntityFrameworkCore;
using MigrationsService.Infrastructure.Persistence;

namespace MigrationsService.Infrastructure;

/// <inheritdoc />
public class MigrationRunner : IMigrationRunner
{
    private readonly MigrationDbContext _context;

    /// <summary>
    /// Предоставляет механизм применения миграций базы данных
    /// </summary>
    /// <param name="context"></param>
    public MigrationRunner(MigrationDbContext context)
    {
        _context = context;
    }

    /// <param name="ct"></param>
    /// <inheritdoc />
    public async Task ApplyMigrationsAsync(CancellationToken ct)
    {
        await _context.Database.MigrateAsync();
    }
}
