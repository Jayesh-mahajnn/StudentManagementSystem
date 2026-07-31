using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;

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

    public async Task<PagedResult<Department>> GetPagedAsync(PaginationParams paginationParams)
    {
        var query = _dbSet.Include(d => d.Courses).Include(d => d.Students).AsQueryable();

        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var term = paginationParams.SearchTerm.Trim().ToLower();
            query = query.Where(d =>
                d.Name.ToLower().Contains(term) ||
                d.Code.ToLower().Contains(term));
        }

        query = paginationParams.SortBy?.ToLower() switch
        {
            "name" => paginationParams.SortDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
            "code" => paginationParams.SortDescending ? query.OrderByDescending(d => d.Code) : query.OrderBy(d => d.Code),
            _ => query.OrderBy(d => d.Id)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Department>
        {
            Items = items,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<int> GetTotalCountAsync() => await _dbSet.CountAsync();
}