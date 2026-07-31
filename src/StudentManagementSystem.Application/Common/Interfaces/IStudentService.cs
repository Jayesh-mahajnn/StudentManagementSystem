using StudentManagementSystem.Application.DTOs.Student;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IStudentService
{
    Task<IReadOnlyList<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<bool> UpdateAsync(int id, UpdateStudentDto dto);
    Task<bool> DeleteAsync(int id);
}