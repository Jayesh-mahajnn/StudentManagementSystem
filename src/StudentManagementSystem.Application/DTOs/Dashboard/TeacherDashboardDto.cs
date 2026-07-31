namespace StudentManagementSystem.Application.DTOs.Dashboard;

public class TeacherDashboardDto
{
    public string TeacherName { get; set; } = string.Empty;
    public int TotalAssignmentsCreated { get; set; }
    public int UpcomingTimetableSlotsToday { get; set; }
    public List<RecentAnnouncementDto> RecentAnnouncements { get; set; } = new();
}