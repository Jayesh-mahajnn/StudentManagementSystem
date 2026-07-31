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
}