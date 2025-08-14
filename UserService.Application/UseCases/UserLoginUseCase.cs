using UserService.Domain.Models;

namespace UserService.Application.UseCases;

public class UserLoginUseCase
{
    private readonly IUserRepository _userRepository;
    
    public UserLoginUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> ExecuteAsync(string username, string password)
    {
        var user = await _userRepository.GetByNameAsync(username);

        if (user == null)
        {
            throw new Exception("Пользователь с таким именем не найден.");
        }

        if (!user.IsPasswordCorrect(password))
        {
            throw new Exception("Неверный пароль.");
        }

        return user;
    }
}