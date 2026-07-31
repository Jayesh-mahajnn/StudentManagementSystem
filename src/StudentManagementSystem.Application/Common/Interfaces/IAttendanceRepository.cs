using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IReadOnlyList<Attendance>> GetBySubjectAndDateAsync(int subjectId, DateTime date);
    Task<IReadOnlyList<Attendance>> GetByStudentAsync(int studentId);
    Task AddRangeAsync(IEnumerable<Attendance> attendances);

    Task<(int totalRecords, int presentRecords)> GetOverallAttendanceStatsAsync();
    Task<IReadOnlyList<Attendance>> GetByTeacherAndDateAsync(int teacherId, DateTime date);

}