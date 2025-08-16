namespace UserService.Application.Contracts;

public sealed record LoginUserCommand(string Name, string Password);