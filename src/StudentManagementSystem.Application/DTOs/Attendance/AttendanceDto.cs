namespace StudentManagementSystem.Application.DTOs.Attendance;

public class AttendanceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string MarkedByTeacherName { get; set; } = string.Empty;
}