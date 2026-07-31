using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByEnrollmentNumberAsync(string enrollmentNumber);
    Task<IReadOnlyList<Student>> GetByDepartmentIdAsync(int departmentId);

    Task<IReadOnlyList<Student>> GetAllWithDetailsAsync();
    Task<Student?> GetByIdWithDetailsAsync(int id);
}