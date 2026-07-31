namespace StudentManagementSystem.Application.DTOs.Auth;

public class RegisterDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Admin" | "Teacher" | "Student"

    // Only required when Role = "Student"
    public string? EnrollmentNumber { get; set; }

    // Only required when Role = "Teacher" — must match an existing Teacher's email
    public string? TeacherEmail { get; set; }
}