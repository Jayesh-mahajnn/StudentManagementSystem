using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;
namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Department>> GetAllWithDetailsAsync();
    Task<Department?> GetByIdWithDetailsAsync(int id);

    Task<PagedResult<Department>> GetPagedAsync(PaginationParams paginationParams);
}