using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Subject>> GetByCourseIdAsync(int courseId) =>
        await _dbSet.Where(s => s.CourseId == courseId).ToListAsync();
}