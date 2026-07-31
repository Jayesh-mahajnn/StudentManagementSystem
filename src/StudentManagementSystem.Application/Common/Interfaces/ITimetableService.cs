using StudentManagementSystem.Application.DTOs.Timetable;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITimetableService
{
    Task<IReadOnlyList<TimetableDto>> GetAllAsync();
    Task<TimetableDto?> GetByIdAsync(int id);
    Task<TimetableDto> CreateAsync(CreateTimetableDto dto);
    Task<bool> UpdateAsync(int id, UpdateTimetableDto dto);
    Task<bool> DeleteAsync(int id);
}