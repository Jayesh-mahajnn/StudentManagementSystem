using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class MarksRepository : Repository<Marks>, IMarksRepository
{
    public MarksRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Marks>> GetByStudentAsync(int studentId) =>
        await _dbSet.Include(m => m.Subject)
            .Where(m => m.StudentId == studentId)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<Marks> marks) =>
        await _dbSet.AddRangeAsync(marks);
}