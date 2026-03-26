using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterDto dto);
    Task<User> LoginAsync(LoginDto dto);
    Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    Task UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task<User> HandleExternalLoginAsync(string email, string name);
}
