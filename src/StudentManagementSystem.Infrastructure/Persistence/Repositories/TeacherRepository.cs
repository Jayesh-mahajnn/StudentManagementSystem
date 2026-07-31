using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;


namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context) { }

    public async Task<Teacher?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(t => t.Email == email);

    public async Task<IReadOnlyList<Teacher>> GetAllWithDetailsAsync() =>
    await _dbSet.Include(t => t.Department).ToListAsync();

    public async Task<Teacher?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet.Include(t => t.Department).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<PagedResult<Teacher>> GetPagedAsync(PaginationParams paginationParams)
    {
        var query = _dbSet.Include(t => t.Department).AsQueryable();

        if (paginationParams.DepartmentId.HasValue)
            query = query.Where(t => t.DepartmentId == paginationParams.DepartmentId.Value);

        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var term = paginationParams.SearchTerm.Trim().ToLower();
            query = query.Where(t =>
                t.FullName.ToLower().Contains(term) ||
                t.Email.ToLower().Contains(term));
        }

        query = paginationParams.SortBy?.ToLower() switch
        {
            "fullname" => paginationParams.SortDescending ? query.OrderByDescending(t => t.FullName) : query.OrderBy(t => t.FullName),
            "email" => paginationParams.SortDescending ? query.OrderByDescending(t => t.Email) : query.OrderBy(t => t.Email),
            "dateofjoining" => paginationParams.SortDescending ? query.OrderByDescending(t => t.DateOfJoining) : query.OrderBy(t => t.DateOfJoining),
            _ => query.OrderBy(t => t.Id)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Teacher>
        {
            Items = items,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize,
            TotalCount = totalCount
        };
    }
}