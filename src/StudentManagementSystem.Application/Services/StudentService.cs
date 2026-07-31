using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync()
    {
        var students = await _unitOfWork.Students.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdWithDetailsAsync(id);
        return student is null ? null : _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var student = _mapper.Map<Student>(dto);
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Students.GetByIdWithDetailsAsync(student.Id);
        return _mapper.Map<StudentDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateStudentDto dto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student is null) return false;

        student.FullName = dto.FullName;
        student.Phone = dto.Phone;
        student.DepartmentId = dto.DepartmentId;
        student.CourseId = dto.CourseId;
        student.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Students.Update(student);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student is null) return false;

        student.IsDeleted = true;
        _unitOfWork.Students.Update(student);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}