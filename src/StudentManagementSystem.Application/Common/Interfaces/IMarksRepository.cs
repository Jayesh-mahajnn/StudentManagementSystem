using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IMarksRepository : IRepository<Marks>
{
    Task<IReadOnlyList<Marks>> GetByStudentAsync(int studentId);
    Task AddRangeAsync(IEnumerable<Marks> marks);
}