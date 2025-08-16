using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;
    public UserRepository(UserDbContext db) => _db = db;
    
    public async Task<bool> ExistsByNameAsync(string name)
        => await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Name == name);


    public async Task<User?> GetByNameAsync(string name)
        => await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Name == name);

    public async Task AddNewUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}