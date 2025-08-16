namespace UserService.Application.Contracts;

public sealed record RegisterUserCommand(string Name, string Password, string ConfirmPassword);