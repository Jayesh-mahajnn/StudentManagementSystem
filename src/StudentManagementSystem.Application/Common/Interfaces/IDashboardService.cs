using StudentManagementSystem.Application.DTOs.Dashboard;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboardDto> GetAdminDashboardAsync();
    Task<TeacherDashboardDto> GetTeacherDashboardAsync(int teacherId);
    Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId);
}