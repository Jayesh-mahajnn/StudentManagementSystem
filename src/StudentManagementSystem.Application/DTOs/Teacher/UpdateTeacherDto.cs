namespace StudentManagementSystem.Application.DTOs.Teacher;

public class UpdateTeacherDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}