using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g. "CS301"
    public int Credits { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}