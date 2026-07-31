using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Announcement;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AnnouncementService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AnnouncementDto>> GetAllAsync()
    {
        var items = await _unitOfWork.Announcements.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<AnnouncementDto>>(items);
    }

    public async Task<AnnouncementDto?> GetByIdAsync(int id)
    {
        var item = await _unitOfWork.Announcements.GetByIdWithDetailsAsync(id);
        return item is null ? null : _mapper.Map<AnnouncementDto>(item);
    }

    public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, int postedByUserId)
    {
        var entity = _mapper.Map<Announcement>(dto);
        entity.PostedByUserId = postedByUserId;   // set here, NOT from the DTO
        entity.PublishedAt = DateTime.UtcNow;

        await _unitOfWork.Announcements.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Announcements.GetByIdWithDetailsAsync(entity.Id);
        return _mapper.Map<AnnouncementDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto)
    {
        var entity = await _unitOfWork.Announcements.GetByIdAsync(id);
        if (entity is null) return false;

        entity.Title = dto.Title;
        entity.Message = dto.Message;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Announcements.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Announcements.GetByIdAsync(id);
        if (entity is null) return false;

        entity.IsDeleted = true;
        _unitOfWork.Announcements.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}