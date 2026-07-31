using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Assignment>> GetAllWithDetailsAsync() =>
        await _dbSet.Include(a => a.Subject).Include(a => a.Teacher).ToListAsync();

    public async Task<Assignment?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet.Include(a => a.Subject).Include(a => a.Teacher).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<int> GetCountByTeacherAsync(int teacherId) =>
    await _dbSet.CountAsync(a => a.TeacherId == teacherId);
}