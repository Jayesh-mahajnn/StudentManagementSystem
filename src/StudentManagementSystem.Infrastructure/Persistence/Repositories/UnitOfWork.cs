using StudentManagementSystem.Application.Common.Interfaces;


namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IStudentRepository? _students;
    private ITeacherRepository? _teachers;
    private ICourseRepository? _courses;
    private IDepartmentRepository? _departments;
    private ISubjectRepository? _subjects;
    private IUserRepository? _users;              // ← add here
    private IRefreshTokenRepository? _refreshTokens;
    private ITimetableRepository? _timetables;
    private IAssignmentRepository? _assignments;
    private IAnnouncementRepository? _announcements;
    private IAttendanceRepository? _attendances;
    private IMarksRepository? _marksRecords;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IStudentRepository Students => _students ??= new StudentRepository(_context);
    public ITeacherRepository Teachers => _teachers ??= new TeacherRepository(_context);
    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
    public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);
    public ISubjectRepository Subjects => _subjects ??= new SubjectRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);   // ← add here

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();

    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);

    public ITimetableRepository Timetables => _timetables ??= new TimetableRepository(_context);
    public IAssignmentRepository Assignments => _assignments ??= new AssignmentRepository(_context);
    public IAnnouncementRepository Announcements => _announcements ??= new AnnouncementRepository(_context);

    public IAttendanceRepository Attendances => _attendances ??= new AttendanceRepository(_context);
    public IMarksRepository MarksRecords => _marksRecords ??= new MarksRepository(_context);
}