using BookMyTable.DTOs;
using BookMyTable.Models;
using BookMyTable.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTable.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    public static User user = new();
    [HttpPost("register")]
    public async Task<ActionResult<User>> register([FromBody] UserDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user==null)
        {
            return BadRequest("Username already exists, please choose another.");
        }
        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> login([FromBody] UserDto request)
    {
        var res = await authService.LoginAsync(request);
        if (res==null)
        {
            return BadRequest("Wrong username or password.");
        }
        return Ok(res);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        var resualt = await authService.RefreshTokenAsync(request);
        if (resualt is null||resualt.RefreshToken is null || resualt.AccessToken is null)
            return Unauthorized("Invalid refresh token!!!");
        return Ok(resualt);
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("Welcome!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("Welcome Admin!");
    }
}
