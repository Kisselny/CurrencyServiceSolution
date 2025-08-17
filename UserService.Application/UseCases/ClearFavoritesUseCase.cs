using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases;

public class ClearFavoritesUseCase
{
    private readonly IFavoritesRepository _favoritesRepository;
    public ClearFavoritesUseCase(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }

    public async Task<int> Handle(ClearFavoritesCommand cmd, CancellationToken ct = default)
    {
        if (cmd.UserId <= 0)
        {
            throw new ArgumentException("Invalid user id");
        }
        return await _favoritesRepository.ClearAsync(cmd.UserId, ct);
    }
}