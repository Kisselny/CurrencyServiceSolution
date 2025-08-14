using UserService.Domain.Models;

namespace UserService.Application;

public interface IUserRepository
{
    Task<bool> ExistsByNameAsync(string name);
    Task AddNewUserAsync(User user);
}