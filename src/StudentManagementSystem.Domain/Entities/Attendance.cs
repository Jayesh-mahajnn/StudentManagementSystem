using StudentManagementSystem.Domain.Common;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class Attendance : BaseEntity
{
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public int MarkedByTeacherId { get; set; }
    public Teacher MarkedByTeacher { get; set; } = null!;
}