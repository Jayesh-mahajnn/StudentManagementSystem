using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByEnrollmentNumberAsync(string enrollmentNumber);
    Task<IReadOnlyList<Student>> GetByDepartmentIdAsync(int departmentId);

    Task<IReadOnlyList<Student>> GetAllWithDetailsAsync();
    Task<Student?> GetByIdWithDetailsAsync(int id);

    Task<PagedResult<Student>> GetPagedAsync(PaginationParams paginationParams);
}