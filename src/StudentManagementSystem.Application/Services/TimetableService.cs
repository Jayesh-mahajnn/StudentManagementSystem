using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Timetable;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Services;

public class TimetableService : ITimetableService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TimetableService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TimetableDto>> GetAllAsync()
    {
        var items = await _unitOfWork.Timetables.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<TimetableDto>>(items);
    }

    public async Task<TimetableDto?> GetByIdAsync(int id)
    {
        var item = await _unitOfWork.Timetables.GetByIdWithDetailsAsync(id);
        return item is null ? null : _mapper.Map<TimetableDto>(item);
    }

    public async Task<TimetableDto> CreateAsync(CreateTimetableDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.Timetable>(dto);
        await _unitOfWork.Timetables.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Timetables.GetByIdWithDetailsAsync(entity.Id);
        return _mapper.Map<TimetableDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTimetableDto dto)
    {
        var entity = await _unitOfWork.Timetables.GetByIdAsync(id);
        if (entity is null) return false;

        entity.DayOfWeek = Enum.Parse<DayOfWeekEnum>(dto.DayOfWeek, true);
        entity.StartTime = dto.StartTime;
        entity.EndTime = dto.EndTime;
        entity.TeacherId = dto.TeacherId;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Timetables.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Timetables.GetByIdAsync(id);
        if (entity is null) return false;

        entity.IsDeleted = true;
        _unitOfWork.Timetables.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}