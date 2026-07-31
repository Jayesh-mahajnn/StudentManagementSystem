namespace StudentManagementSystem.Application.DTOs.Attendance;

public class AttendanceSummaryDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TotalClasses { get; set; }
    public int PresentCount { get; set; }
    public double AttendancePercentage { get; set; }
}