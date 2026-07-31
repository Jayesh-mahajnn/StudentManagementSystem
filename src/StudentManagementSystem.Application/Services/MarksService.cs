using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Marks;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Services;

public class MarksService : IMarksService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MarksService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MarksDto>> CreateBulkMarksAsync(BulkCreateMarksDto dto)
    {
        var examType = Enum.Parse<ExamType>(dto.ExamType, true);

        var entities = dto.Entries.Select(entry => new Marks
        {
            StudentId = entry.StudentId,
            SubjectId = dto.SubjectId,
            ExamType = examType,
            ObtainedMarks = entry.ObtainedMarks,
            MaxMarks = dto.MaxMarks,
            ExamDate = dto.ExamDate,
            RecordedByTeacherId = dto.TeacherId
        }).ToList();

        await _unitOfWork.MarksRecords.AddRangeAsync(entities);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<IReadOnlyList<MarksDto>>(entities);
    }

    public async Task<IReadOnlyList<MarksDto>> GetByStudentAsync(int studentId)
    {
        var items = await _unitOfWork.MarksRecords.GetByStudentAsync(studentId);
        return _mapper.Map<IReadOnlyList<MarksDto>>(items);
    }
}