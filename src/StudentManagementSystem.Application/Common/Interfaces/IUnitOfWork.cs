namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    ITeacherRepository Teachers { get; }
    ICourseRepository Courses { get; }
    IDepartmentRepository Departments { get; }
    ISubjectRepository Subjects { get; }

    Task<int> SaveChangesAsync();
}