using StudentManagementSystem.Domain.Common;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class Timetable : BaseEntity
{
    public DayOfWeekEnum DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
}