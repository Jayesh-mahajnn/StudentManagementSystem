using StudentManagementSystem.Application.DTOs.Assignment;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAssignmentService
{
    Task<IReadOnlyList<AssignmentDto>> GetAllAsync();
    Task<AssignmentDto?> GetByIdAsync(int id);
    Task<AssignmentDto> CreateAsync(CreateAssignmentDto dto);
    Task<bool> UpdateAsync(int id, UpdateAssignmentDto dto);
    Task<bool> DeleteAsync(int id);
}