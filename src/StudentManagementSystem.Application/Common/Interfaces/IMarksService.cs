using StudentManagementSystem.Application.DTOs.Marks;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IMarksService
{
    Task<IReadOnlyList<MarksDto>> CreateBulkMarksAsync(BulkCreateMarksDto dto);
    Task<IReadOnlyList<MarksDto>> GetByStudentAsync(int studentId);
}