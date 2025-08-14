using Microsoft.EntityFrameworkCore;

namespace MigrationsService.Infrastructure;

public class MigrationRunner : IMigrationRunner
{
    private readonly MigrationDbContext _context;

    public MigrationRunner(MigrationDbContext context)
    {
        _context = context;
    }

    public async Task ApplyMigrationsAsync()
    {
        await _context.Database.MigrateAsync();
    }
}
