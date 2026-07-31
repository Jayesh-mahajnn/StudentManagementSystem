using StudentManagementSystem.Application.DTOs.Attendance;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAttendanceService
{
    Task<IReadOnlyList<AttendanceDto>> MarkBulkAttendanceAsync(BulkMarkAttendanceDto dto);
    Task<IReadOnlyList<AttendanceDto>> GetBySubjectAndDateAsync(int subjectId, DateTime date);
    Task<AttendanceSummaryDto> GetStudentSummaryAsync(int studentId);
}