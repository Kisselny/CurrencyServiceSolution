using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("internal/users")]
//[ApiExplorerSettings(IgnoreApi = true)] //это позже
public class UserFavoritesInternalController : ControllerBase
{
    [HttpGet("{id:int}/favorites")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetFavorites(
        [FromRoute] int id,
        [FromServices] IFavoritesRepository repo,
        CancellationToken ct)
    {
        if (id <= 0)
        {
            return BadRequest(new { error = "UserId должен быть положительным." });
        }

        var list = await repo.InternalGetByUserAsync(id, ct);
        
        var normalized = list
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToUpperInvariant())
            .Distinct()
            .ToArray();

        return Ok(normalized);
    }
}