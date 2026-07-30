using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Course>> GetByDepartmentIdAsync(int departmentId) =>
        await _dbSet.Where(c => c.DepartmentId == departmentId).ToListAsync();
}