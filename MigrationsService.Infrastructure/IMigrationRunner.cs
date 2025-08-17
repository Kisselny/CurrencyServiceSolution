using Microsoft.Extensions.Logging;

using System.Threading;

namespace MigrationsService.Infrastructure;

/// Предоставляет механизм применения миграций базы данных
public interface IMigrationRunner
{
    /// Асинхронно применяет все ожидающие миграции базы данных
    /// <param name="ct"></param>
    Task ApplyMigrationsAsync(CancellationToken ct);
}
