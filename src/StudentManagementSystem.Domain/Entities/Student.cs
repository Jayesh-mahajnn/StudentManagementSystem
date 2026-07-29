using StudentManagementSystem.Domain.Common;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class Student : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EnrollmentNumber { get; set; } = string.Empty; // unique

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}