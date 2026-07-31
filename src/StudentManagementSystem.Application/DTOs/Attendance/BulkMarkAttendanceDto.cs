namespace StudentManagementSystem.Application.DTOs.Attendance;

public class BulkMarkAttendanceDto
{
    public int SubjectId { get; set; }
    public DateTime Date { get; set; }
    public int TeacherId { get; set; }
    public List<MarkAttendanceDto> Entries { get; set; } = new();
}