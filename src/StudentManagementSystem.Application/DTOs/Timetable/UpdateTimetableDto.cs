namespace StudentManagementSystem.Application.DTOs.Timetable;

public class UpdateTimetableDto
{
    public string DayOfWeek { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int TeacherId { get; set; }
}