using StudentManagementSystem.Application.DTOs.Teacher;
using StudentManagementSystem.Application.Common.Models;
namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITeacherService
{
    Task<IReadOnlyList<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(CreateTeacherDto dto);
    Task<bool> UpdateAsync(int id, UpdateTeacherDto dto);
    Task<bool> DeleteAsync(int id);

    Task<PagedResult<TeacherDto>> GetPagedAsync(PaginationParams paginationParams);
}