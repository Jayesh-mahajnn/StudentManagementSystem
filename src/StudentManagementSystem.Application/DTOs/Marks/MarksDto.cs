namespace StudentManagementSystem.Application.DTOs.Marks;

public class MarksDto
{
    public int Id { get; set; }
    public string ExamType { get; set; } = string.Empty;
    public decimal ObtainedMarks { get; set; }
    public decimal MaxMarks { get; set; }
    public DateTime ExamDate { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}