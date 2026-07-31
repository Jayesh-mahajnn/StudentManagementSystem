using StudentManagementSystem.Domain.Common;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class Marks : BaseEntity
{
    public ExamType ExamType { get; set; }
    public decimal ObtainedMarks { get; set; }
    public decimal MaxMarks { get; set; }
    public DateTime ExamDate { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public int RecordedByTeacherId { get; set; }
    public Teacher RecordedByTeacher { get; set; } = null!;
}