using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Department;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdWithDetailsAsync(id);
        return department is null ? null : _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null) return false;

        department.Name = dto.Name;
        department.Code = dto.Code;
        department.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Departments.Update(department);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null) return false;

        department.IsDeleted = true;
        _unitOfWork.Departments.Update(department);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}