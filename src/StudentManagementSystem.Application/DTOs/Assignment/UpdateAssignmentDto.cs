namespace StudentManagementSystem.Application.DTOs.Assignment;

public class UpdateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string? AttachmentUrl { get; set; }
}