namespace MigrationsService.Domain.Models;

/// <summary>
/// Представляет пользователя в системе
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; init; }
    private string _password;
}