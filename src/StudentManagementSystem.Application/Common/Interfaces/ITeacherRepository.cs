using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITeacherRepository : IRepository<Teacher>
{
    Task<Teacher?> GetByEmailAsync(string email);

    Task<IReadOnlyList<Teacher>> GetAllWithDetailsAsync();
    Task<Teacher?> GetByIdWithDetailsAsync(int id);

    Task<PagedResult<Teacher>> GetPagedAsync(PaginationParams paginationParams);
    Task<int> GetTotalCountAsync();

}