using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class TimetableRepository : Repository<Timetable>, ITimetableRepository
{
    public TimetableRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Timetable>> GetAllWithDetailsAsync() =>
        await _dbSet.Include(t => t.Course).Include(t => t.Subject).Include(t => t.Teacher).ToListAsync();

    public async Task<Timetable?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet.Include(t => t.Course).Include(t => t.Subject).Include(t => t.Teacher)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> IsSlotTakenAsync(int teacherId, DayOfWeekEnum day, TimeSpan startTime, int? excludeId = null)
    {
        var query = _dbSet.Where(t =>
            t.TeacherId == teacherId && t.DayOfWeek == day && t.StartTime == startTime);

        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value); // exclude self when checking during an Update

        return await query.AnyAsync();
    }

    public async Task<int> GetCountForTeacherOnDayAsync(int teacherId, Domain.Enums.DayOfWeekEnum day) =>
    await _dbSet.CountAsync(t => t.TeacherId == teacherId && t.DayOfWeek == day);
}