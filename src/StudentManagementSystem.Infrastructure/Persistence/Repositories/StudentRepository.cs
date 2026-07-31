using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context) { }

    public async Task<Student?> GetByEnrollmentNumberAsync(string enrollmentNumber) =>
        await _dbSet.FirstOrDefaultAsync(s => s.EnrollmentNumber == enrollmentNumber);

    public async Task<IReadOnlyList<Student>> GetByDepartmentIdAsync(int departmentId) =>
        await _dbSet.Where(s => s.DepartmentId == departmentId).ToListAsync();

    public async Task<IReadOnlyList<Student>> GetAllWithDetailsAsync() =>
    await _dbSet.Include(s => s.Department).Include(s => s.Course).ToListAsync();

    public async Task<Student?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet.Include(s => s.Department).Include(s => s.Course).FirstOrDefaultAsync(s => s.Id == id);
}