using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ISubjectRepository : IRepository<Subject>
{
    Task<IReadOnlyList<Subject>> GetByCourseIdAsync(int courseId);
}