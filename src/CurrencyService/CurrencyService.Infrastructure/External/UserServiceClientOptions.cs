namespace CurrencyService.Infrastructure.External;

/// Параметры конфигурации для клиента API сервиса пользователей
public sealed class UserServiceClientOptions
{
    /// Базовый URL для взаимодействия с удалённым микросервисом
    public string BaseUrl { get; init; } = null!;

    /// Ключ для аутентификации при обращении к сервису пользователей
    public string? ServiceKey { get; init; }
}