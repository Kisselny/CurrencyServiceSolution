namespace CurrencyUpdaterService.Infrastructure.External;

public class CurrencyDto
{
    public string Id { get; set; } = null!;
    public string CharCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Nominal { get; set; }
    public decimal Value { get; set; }

}