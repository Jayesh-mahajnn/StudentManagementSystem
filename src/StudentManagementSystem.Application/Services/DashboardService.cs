using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Dashboard;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync()
    {
        // Independent queries — run concurrently since none depends on another's result
        var studentsCountTask = _unitOfWork.Students.GetTotalCountAsync();
        var teachersCountTask = _unitOfWork.Teachers.GetTotalCountAsync();
        var departmentsCountTask = _unitOfWork.Departments.GetTotalCountAsync();
        var coursesCountTask = _unitOfWork.Courses.GetTotalCountAsync();
        var attendanceStatsTask = _unitOfWork.Attendances.GetOverallAttendanceStatsAsync();
        var recentAnnouncementsTask = _unitOfWork.Announcements.GetRecentAsync(5);

        await Task.WhenAll(
            studentsCountTask, teachersCountTask, departmentsCountTask,
            coursesCountTask, attendanceStatsTask, recentAnnouncementsTask);

        var (totalRecords, presentRecords) = attendanceStatsTask.Result;

        return new AdminDashboardDto
        {
            TotalStudents = studentsCountTask.Result,
            TotalTeachers = teachersCountTask.Result,
            TotalDepartments = departmentsCountTask.Result,
            TotalCourses = coursesCountTask.Result,
            OverallAttendancePercentage = totalRecords == 0 ? 0 : Math.Round((double)presentRecords / totalRecords * 100, 2),
            RecentAnnouncements = recentAnnouncementsTask.Result
                .Select(a => new RecentAnnouncementDto { Title = a.Title, PublishedAt = a.PublishedAt })
                .ToList()
        };
    }

    public async Task<TeacherDashboardDto> GetTeacherDashboardAsync(int teacherId)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(teacherId)
            ?? throw new StudentManagementSystem.Shared.Exceptions.NotFoundException("Teacher", teacherId);

        var today = DateTime.UtcNow.DayOfWeek switch
        {
            System.DayOfWeek.Monday => DayOfWeekEnum.Monday,
            System.DayOfWeek.Tuesday => DayOfWeekEnum.Tuesday,
            System.DayOfWeek.Wednesday => DayOfWeekEnum.Wednesday,
            System.DayOfWeek.Thursday => DayOfWeekEnum.Thursday,
            System.DayOfWeek.Friday => DayOfWeekEnum.Friday,
            System.DayOfWeek.Saturday => DayOfWeekEnum.Saturday,
            _ => DayOfWeekEnum.Monday // Sunday has no classes in our schema; default fallback
        };

        var assignmentsCountTask = _unitOfWork.Assignments.GetCountByTeacherAsync(teacherId);
        var todaysSlotsTask = _unitOfWork.Timetables.GetCountForTeacherOnDayAsync(teacherId, today);
        var recentAnnouncementsTask = _unitOfWork.Announcements.GetRecentAsync(5);

        await Task.WhenAll(assignmentsCountTask, todaysSlotsTask, recentAnnouncementsTask);

        return new TeacherDashboardDto
        {
            TeacherName = teacher.FullName,
            TotalAssignmentsCreated = assignmentsCountTask.Result,
            UpcomingTimetableSlotsToday = todaysSlotsTask.Result,
            RecentAnnouncements = recentAnnouncementsTask.Result
                .Select(a => new RecentAnnouncementDto { Title = a.Title, PublishedAt = a.PublishedAt })
                .ToList()
        };
    }

    public async Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId)
            ?? throw new StudentManagementSystem.Shared.Exceptions.NotFoundException("Student", studentId);

        var attendanceRecordsTask = _unitOfWork.Attendances.GetByStudentAsync(studentId);
        var recentAnnouncementsTask = _unitOfWork.Announcements.GetRecentAsync(5);

        await Task.WhenAll(attendanceRecordsTask, recentAnnouncementsTask);

        var records = attendanceRecordsTask.Result;
        var total = records.Count;
        var present = records.Count(r => r.Status == AttendanceStatus.Present);

        return new StudentDashboardDto
        {
            StudentName = student.FullName,
            AttendancePercentage = total == 0 ? 0 : Math.Round((double)present / total * 100, 2),
            TotalAssignmentsPending = 0, // Flagged below — see note
            RecentAnnouncements = recentAnnouncementsTask.Result
                .Select(a => new RecentAnnouncementDto { Title = a.Title, PublishedAt = a.PublishedAt })
                .ToList()
        };
    }
}