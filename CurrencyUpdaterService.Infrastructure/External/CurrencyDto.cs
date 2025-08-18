namespace CurrencyUpdaterService.Infrastructure.External;

/// DTO валюты
public class CurrencyDto
{
    /// Уникальный идентификатор валюты
    public string Id { get; set; } = null!;

    /// Буквенный код валюты
    public string CharCode { get; set; } = null!;

    /// Наименование валюты
    public string Name { get; set; } = null!;

    /// Номинал валюты
    public int Nominal { get; set; }

    /// Курс в рублях относительно номинала валюты
    public decimal Value { get; set; }

}