namespace StudentManagementSystem.Application.DTOs.Teacher;

public class CreateTeacherDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public int DepartmentId { get; set; }
}