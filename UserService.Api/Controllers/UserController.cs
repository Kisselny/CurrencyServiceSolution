using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts;
using UserService.Application.UseCases;

namespace UserService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserLoginUseCase _loginUser;

    public UserController(ILogger<UserController> logger, UserLoginUseCase loginUser)
    {
        _logger = logger;
        _loginUser = loginUser;
    }

    // [AllowAnonymous]
    // [HttpPost("register")]
    // public IActionResult Register(string name, string password, string confirmPassword)
    // {
    // }
    //
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResult>> Login([FromBody] LoginUserCommand command, CancellationToken ct)
    {
        try
        {
            var result = await _loginUser.ExecuteAsync(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    //test shit
    [Authorize]
    [HttpGet("me/ping")]
    public ActionResult<object> MePing()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
        var userName = User.Identity?.Name 
                       ?? User.FindFirstValue("unique_name");
        
        return Ok(new
        {
            ok = true,
            userId,
            userName
        });
    }

    // [Authorize]
    // [HttpGet("me/favorites")]
    // public IActionResult GetFavorites()
    // {
    //     var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
    //                            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
    // }
    
    
}