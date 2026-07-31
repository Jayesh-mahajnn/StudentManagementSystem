using StudentManagementSystem.Domain.Entities;
namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAnnouncementRepository : IRepository<Announcement>
{
    Task<IReadOnlyList<Announcement>> GetAllWithDetailsAsync();
    Task<Announcement?> GetByIdWithDetailsAsync(int id);
}