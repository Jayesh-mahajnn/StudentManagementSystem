using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context) { }

    public async Task<Teacher?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(t => t.Email == email);
}