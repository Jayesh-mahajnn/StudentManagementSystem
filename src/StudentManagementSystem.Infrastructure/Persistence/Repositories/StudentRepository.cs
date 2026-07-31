using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Application.Common.Models;

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

    public async Task<PagedResult<Student>> GetPagedAsync(PaginationParams paginationParams)
    {
        var query = _dbSet
            .Include(s => s.Department)
            .Include(s => s.Course)
            .AsQueryable();

        // Filter
        if (paginationParams.DepartmentId.HasValue)
            query = query.Where(s => s.DepartmentId == paginationParams.DepartmentId.Value);

        // Search (across FullName, Email, EnrollmentNumber)
        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var term = paginationParams.SearchTerm.Trim().ToLower();
            query = query.Where(s =>
                s.FullName.ToLower().Contains(term) ||
                s.Email.ToLower().Contains(term) ||
                s.EnrollmentNumber.ToLower().Contains(term));
        }

        // Sort (whitelist allowed columns — never trust a raw column name from the client)
        query = paginationParams.SortBy?.ToLower() switch
        {
            "fullname" => paginationParams.SortDescending ? query.OrderByDescending(s => s.FullName) : query.OrderBy(s => s.FullName),
            "enrollmentnumber" => paginationParams.SortDescending ? query.OrderByDescending(s => s.EnrollmentNumber) : query.OrderBy(s => s.EnrollmentNumber),
            "dateofbirth" => paginationParams.SortDescending ? query.OrderByDescending(s => s.DateOfBirth) : query.OrderBy(s => s.DateOfBirth),
            _ => query.OrderBy(s => s.Id) // default, stable sort
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Student>
        {
            Items = items,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize,
            TotalCount = totalCount
        };
    }
}