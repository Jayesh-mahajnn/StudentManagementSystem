using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<Department?> GetByCodeAsync(string code) =>
        await _dbSet.FirstOrDefaultAsync(d => d.Code == code);

    public async Task<IReadOnlyList<Department>> GetAllWithDetailsAsync() =>
        await _dbSet
            .Include(d => d.Courses)
            .Include(d => d.Students)
            .ToListAsync();

    public async Task<Department?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet
            .Include(d => d.Courses)
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == id);
}