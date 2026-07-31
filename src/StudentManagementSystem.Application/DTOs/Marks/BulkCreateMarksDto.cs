namespace StudentManagementSystem.Application.DTOs.Marks;

public class BulkCreateMarksDto
{
    public int SubjectId { get; set; }
    public string ExamType { get; set; } = string.Empty;
    public decimal MaxMarks { get; set; }
    public DateTime ExamDate { get; set; }
    public int TeacherId { get; set; }
    public List<CreateMarksDto> Entries { get; set; } = new();
}