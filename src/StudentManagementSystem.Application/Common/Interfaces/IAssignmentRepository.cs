using StudentManagementSystem.Domain.Entities;
namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<IReadOnlyList<Assignment>> GetAllWithDetailsAsync();
    Task<Assignment?> GetByIdWithDetailsAsync(int id);

    Task<int> GetCountByTeacherAsync(int teacherId);
}