namespace StudentManagementSystem.Application.DTOs.Attendance;

public class MarkAttendanceDto
{
    public int StudentId { get; set; }
    public string Status { get; set; } = string.Empty; // "Present" | "Absent" | "Late" | "Excused"
}