namespace CurrencyService.Infrastructure.External;

public sealed class UserServiceClientOptions
{
    public string BaseUrl { get; init; } = null!;
    
    public string? ServiceKey { get; init; }
}