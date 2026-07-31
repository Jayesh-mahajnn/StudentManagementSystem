using StudentManagementSystem.Application.DTOs.Announcement;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAnnouncementService
{
    Task<IReadOnlyList<AnnouncementDto>> GetAllAsync();
    Task<AnnouncementDto?> GetByIdAsync(int id);
    Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, int postedByUserId);
    Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto);
    Task<bool> DeleteAsync(int id);
}