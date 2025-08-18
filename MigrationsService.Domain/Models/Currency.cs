namespace MigrationsService.Domain.Models;

/// Представляет денежную валюту с обменным курсом
public class Currency
{
    /// <summary>
    /// Идентификатор валюты
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Обменный курс валюты
    /// </summary>
    public decimal Rate { get; set; }
}