using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Persistence;

/// <inheritdoc />
public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;
    /// <summary>
    /// Интерфейс для работы с репозиторием пользователей
    /// </summary>
    /// <param name="db">Контекст БД пользователей</param>
    public UserRepository(UserDbContext db) => _db = db;

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(string name)
        => await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Name == name);


    /// <inheritdoc />
    public async Task<User?> GetByNameAsync(string name)
        => await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Name == name);

    /// <inheritdoc />
    public async Task AddNewUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}