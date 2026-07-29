using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;   // e.g. "B.Tech"
    public int DurationYears { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
}