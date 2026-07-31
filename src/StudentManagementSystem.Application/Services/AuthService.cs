using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Auth;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using BCrypt.Net;

namespace StudentManagementSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private const int RefreshTokenExpiryDays = 7;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existing = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
        if (existing is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        if (!Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var parsedRole))
            throw new ArgumentException($"Invalid role '{dto.Role}'. Must be Admin, Teacher, or Student.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = parsedRole
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);

        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        var user = await _unitOfWork.Users.GetByIdAsync(storedToken.UserId);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        // Rotate: revoke the old token before issuing a new pair
        storedToken.IsRevoked = true;
        _unitOfWork.RefreshTokens.Update(storedToken);

        return await IssueTokensAsync(user);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var storedToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (storedToken is null) return; // already gone; logout is idempotent

        storedToken.IsRevoked = true;
        _unitOfWork.RefreshTokens.Update(storedToken);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> IssueTokensAsync(User user)
    {
        var (accessToken, expiresAt) = _jwtTokenService.GenerateToken(user);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays),
            UserId = user.Id
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = accessToken,
            ExpiresAt = expiresAt,
            RefreshToken = refreshTokenValue
        };
    }
}