using Microsoft.Extensions.Logging;

using System.Threading;

namespace MigrationsService.Infrastructure;

public interface IMigrationRunner
{
    Task ApplyMigrationsAsync();
}
