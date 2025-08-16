namespace MigrationsService.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; init; }
    private string _password;
}