using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<IReadOnlyList<Course>> GetByDepartmentIdAsync(int departmentId);

    Task<int> GetTotalCountAsync();
}