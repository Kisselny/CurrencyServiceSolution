using UserService.Domain.Models;

namespace UserService.Application.UseCases;

public class UserRegistrationUseCase
{
    private readonly IUserRepository _userRepository;
    
    public UserRegistrationUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task ExecuteAsync(string name, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            throw new Exception("Пароль и подтверждение пароля не совпадают.");
        }
        
        if (await _userRepository.ExistsByNameAsync(name))
        {
            throw new Exception("Пользователь с таким именем уже существует.");
        }

        var user = new User(name, password);

        await _userRepository.AddNewUserAsync(user);
    }
}