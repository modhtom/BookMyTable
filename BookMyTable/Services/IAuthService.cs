using BookMyTable.DTOs;
using BookMyTable.Models;

namespace BookMyTable.Services;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<User?> RegisterAsync(UserDto request);

}
