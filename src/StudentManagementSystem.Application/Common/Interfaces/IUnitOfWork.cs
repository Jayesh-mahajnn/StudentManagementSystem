namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    ITeacherRepository Teachers { get; }
    ICourseRepository Courses { get; }
    IDepartmentRepository Departments { get; }
    ISubjectRepository Subjects { get; }

    IUserRepository Users { get; }

    IRefreshTokenRepository RefreshTokens { get; }

    ITimetableRepository Timetables { get; }
    IAssignmentRepository Assignments { get; }
    IAnnouncementRepository Announcements { get; }
    IAttendanceRepository Attendances { get; }
    IMarksRepository MarksRecords { get; }   // named "MarksRecords" to avoid clashing with the DbSet name "Marks"
    Task<int> SaveChangesAsync();
}