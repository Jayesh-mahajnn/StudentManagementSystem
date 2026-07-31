namespace StudentManagementSystem.Application.DTOs.Dashboard;

public class AdminDashboardDto
{
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalCourses { get; set; }
    public double OverallAttendancePercentage { get; set; }
    public List<RecentAnnouncementDto> RecentAnnouncements { get; set; } = new();
}

public class RecentAnnouncementDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
}