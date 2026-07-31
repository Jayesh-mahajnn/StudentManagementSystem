namespace StudentManagementSystem.Application.DTOs.Assignment;

public class CreateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string? AttachmentUrl { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
}