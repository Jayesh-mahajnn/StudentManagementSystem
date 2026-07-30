using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITeacherRepository : IRepository<Teacher>
{
    Task<Teacher?> GetByEmailAsync(string email);
}