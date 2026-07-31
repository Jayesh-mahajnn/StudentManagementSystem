using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Assignment;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AssignmentDto>> GetAllAsync()
    {
        var items = await _unitOfWork.Assignments.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<AssignmentDto>>(items);
    }

    public async Task<AssignmentDto?> GetByIdAsync(int id)
    {
        var item = await _unitOfWork.Assignments.GetByIdWithDetailsAsync(id);
        return item is null ? null : _mapper.Map<AssignmentDto>(item);
    }

    public async Task<AssignmentDto> CreateAsync(CreateAssignmentDto dto)
    {
        var entity = _mapper.Map<Assignment>(dto);
        await _unitOfWork.Assignments.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Assignments.GetByIdWithDetailsAsync(entity.Id);
        return _mapper.Map<AssignmentDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAssignmentDto dto)
    {
        var entity = await _unitOfWork.Assignments.GetByIdAsync(id);
        if (entity is null) return false;

        entity.Title = dto.Title;
        entity.Description = dto.Description;
        entity.DueDate = dto.DueDate;
        entity.AttachmentUrl = dto.AttachmentUrl;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Assignments.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Assignments.GetByIdAsync(id);
        if (entity is null) return false;

        entity.IsDeleted = true;
        _unitOfWork.Assignments.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}