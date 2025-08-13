namespace CurrencyUpdaterService.Worker.Services;

public class CustomServiceScopeFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CustomServiceScopeFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
    
    public async Task RunInScopeAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = _serviceProvider.CreateScope();
        await func(scope.ServiceProvider);
    }
}