using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Attendance>> GetBySubjectAndDateAsync(int subjectId, DateTime date) =>
        await _dbSet.Include(a => a.Student).Include(a => a.Subject).Include(a => a.MarkedByTeacher)
            .Where(a => a.SubjectId == subjectId && a.Date.Date == date.Date)
            .ToListAsync();

    public async Task<IReadOnlyList<Attendance>> GetByStudentAsync(int studentId) =>
        await _dbSet.Include(a => a.Subject)
            .Where(a => a.StudentId == studentId)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<Attendance> attendances) =>
        await _dbSet.AddRangeAsync(attendances);

    public async Task<(int totalRecords, int presentRecords)> GetOverallAttendanceStatsAsync()
    {
        var total = await _dbSet.CountAsync();
        var present = await _dbSet.CountAsync(a => a.Status == Domain.Enums.AttendanceStatus.Present);
        return (total, present);
    }

    public async Task<IReadOnlyList<Attendance>> GetByTeacherAndDateAsync(int teacherId, DateTime date) =>
        await _dbSet.Where(a => a.MarkedByTeacherId == teacherId && a.Date.Date == date.Date).ToListAsync();
}