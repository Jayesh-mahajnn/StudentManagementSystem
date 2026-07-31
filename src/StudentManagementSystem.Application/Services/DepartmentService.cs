using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Common.Models;
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

    public async Task<PagedResult<DepartmentDto>> GetPagedAsync(PaginationParams paginationParams)
    {
        var paged = await _unitOfWork.Departments.GetPagedAsync(paginationParams);

        return new PagedResult<DepartmentDto>
        {
            Items = _mapper.Map<IReadOnlyList<DepartmentDto>>(paged.Items),
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalCount = paged.TotalCount
        };
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