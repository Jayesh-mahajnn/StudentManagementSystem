using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
{
    public AnnouncementRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Announcement>> GetAllWithDetailsAsync() =>
        await _dbSet.Include(a => a.Department).Include(a => a.Course).Include(a => a.PostedByUser)
            .OrderByDescending(a => a.PublishedAt).ToListAsync();

    public async Task<Announcement?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet.Include(a => a.Department).Include(a => a.Course).Include(a => a.PostedByUser)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IReadOnlyList<Announcement>> GetRecentAsync(int count) =>
    await _dbSet.OrderByDescending(a => a.PublishedAt).Take(count).ToListAsync();
}