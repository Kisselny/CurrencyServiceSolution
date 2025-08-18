namespace CurrencyUpdaterService.Worker.Services;

/// Фабрика для создания и управления пользовательскими сервисами
public class CustomServiceScopeFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// Фабрика для создания и управления пользовательскими сервисами
    public CustomServiceScopeFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// Создает новый объект службы
    /// <return>Объект области службы</return>
    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }

    /// Выполняет переданную функцию в области видимости службы
    /// <param name="func">Функция для выполнения, принимающая IServiceProvider</param>
    public async Task RunInScopeAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = _serviceProvider.CreateScope();
        await func(scope.ServiceProvider);
    }
}