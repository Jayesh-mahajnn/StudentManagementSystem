namespace StudentManagementSystem.Application.DTOs.Student;

public class UpdateStudentDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int CourseId { get; set; }
}