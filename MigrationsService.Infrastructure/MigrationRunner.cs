using Microsoft.EntityFrameworkCore;

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

    /// <inheritdoc />
    public async Task ApplyMigrationsAsync()
    {
        await _context.Database.MigrateAsync();
    }
}
