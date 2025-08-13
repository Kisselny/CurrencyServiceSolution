namespace CurrencyUpdaterService.Domain.Models;

public class Currency
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}