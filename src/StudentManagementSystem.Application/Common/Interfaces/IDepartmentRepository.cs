using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Department>> GetAllWithDetailsAsync();
    Task<Department?> GetByIdWithDetailsAsync(int id);
}