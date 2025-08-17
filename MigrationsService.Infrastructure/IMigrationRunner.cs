using Microsoft.Extensions.Logging;

using System.Threading;

namespace MigrationsService.Infrastructure;

/// Предоставляет механизм применения миграций базы данных
public interface IMigrationRunner
{
    /// Асинхронно применяет все ожидающие миграции базы данных
    Task ApplyMigrationsAsync();
}
