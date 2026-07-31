using StudentManagementSystem.Domain.Entities;
namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITimetableRepository : IRepository<Timetable>
{
    Task<IReadOnlyList<Timetable>> GetAllWithDetailsAsync();
    Task<Timetable?> GetByIdWithDetailsAsync(int id);
    Task<bool> IsSlotTakenAsync(int teacherId, Domain.Enums.DayOfWeekEnum day, TimeSpan startTime, int? excludeId = null);

    Task<int> GetCountForTeacherOnDayAsync(int teacherId, Domain.Enums.DayOfWeekEnum day);
}