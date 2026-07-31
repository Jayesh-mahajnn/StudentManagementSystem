namespace StudentManagementSystem.Application.DTOs.Announcement;

public class AnnouncementDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? CourseId { get; set; }
    public string? CourseName { get; set; }
    public string PostedByName { get; set; } = string.Empty;
}