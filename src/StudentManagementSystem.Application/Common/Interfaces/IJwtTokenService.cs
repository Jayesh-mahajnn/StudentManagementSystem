using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IJwtTokenService
{
    (string token, DateTime expiresAt) GenerateToken(User user);

    string GenerateRefreshToken();
}