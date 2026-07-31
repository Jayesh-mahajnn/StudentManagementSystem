namespace StudentManagementSystem.Application.DTOs.Student;

public class CreateStudentDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty; // "Male" | "Female" | "Other"
    public DateTime DateOfBirth { get; set; }
    public string EnrollmentNumber { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int CourseId { get; set; }
}