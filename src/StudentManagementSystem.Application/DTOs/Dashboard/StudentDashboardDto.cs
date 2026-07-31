namespace StudentManagementSystem.Application.DTOs.Dashboard;

public class StudentDashboardDto
{
    public string StudentName { get; set; } = string.Empty;
    public double AttendancePercentage { get; set; }
    public int TotalAssignmentsPending { get; set; }
    public List<RecentAnnouncementDto> RecentAnnouncements { get; set; } = new();
}