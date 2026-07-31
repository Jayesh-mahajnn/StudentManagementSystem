using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Teacher;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeacherDto>> GetAllAsync()
    {
        var teachers = await _unitOfWork.Teachers.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<TeacherDto>>(teachers);
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdWithDetailsAsync(id);
        return teacher is null ? null : _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> CreateAsync(CreateTeacherDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        await _unitOfWork.Teachers.AddAsync(teacher);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Teachers.GetByIdWithDetailsAsync(teacher.Id);
        return _mapper.Map<TeacherDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTeacherDto dto)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher is null) return false;

        teacher.FullName = dto.FullName;
        teacher.Phone = dto.Phone;
        teacher.DepartmentId = dto.DepartmentId;
        teacher.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Teachers.Update(teacher);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher is null) return false;

        teacher.IsDeleted = true;
        _unitOfWork.Teachers.Update(teacher);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}