namespace StudentManagementSystem.Application.DTOs.Timetable;

public class CreateTimetableDto
{
    public string DayOfWeek { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int CourseId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
}