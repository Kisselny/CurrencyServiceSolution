using System.ComponentModel.DataAnnotations;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.UseCases;

public class UserLoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwt;
    
    public UserLoginUseCase(IUserRepository userRepository, IJwtTokenGenerator jwt)
    {
        _userRepository = userRepository;
        _jwt = jwt;
    }
    
    public async Task<LoginUserResult> ExecuteAsync(LoginUserCommand cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd.Name))
            throw new ValidationException("Необходимо ввести имя пользователя.");
        if (string.IsNullOrWhiteSpace(cmd.Password))
            throw new ValidationException("Необходимо ввести пароль.");
        
        var user = await _userRepository.GetByNameAsync(cmd.Name);

        if (user == null)
            throw new Exception("Пользователь с таким именем не найден.");

        if (!user.IsPasswordCorrect(cmd.Password))
            throw new Exception("Неверный пароль.");
        
        var token = _jwt.GenerateToken(user.Id, user.Name);
        
        return new LoginUserResult(token);
    }
}