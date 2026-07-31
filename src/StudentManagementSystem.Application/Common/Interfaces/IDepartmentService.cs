using StudentManagementSystem.Application.Common.Models;
using StudentManagementSystem.Application.DTOs.Department;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentDto>> GetPagedAsync(PaginationParams paginationParams);
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto);
    Task<bool> DeleteAsync(int id);


}