using AutoMapper;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Attendance;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AttendanceDto>> MarkBulkAttendanceAsync(BulkMarkAttendanceDto dto)
    {
        var entities = dto.Entries.Select(entry => new Attendance
        {
            StudentId = entry.StudentId,
            SubjectId = dto.SubjectId,
            Date = dto.Date.Date,
            Status = Enum.Parse<AttendanceStatus>(entry.Status, true),
            MarkedByTeacherId = dto.TeacherId
        });

        await _unitOfWork.Attendances.AddRangeAsync(entities);
        await _unitOfWork.SaveChangesAsync(); // ONE transaction for the whole class

        var saved = await _unitOfWork.Attendances.GetBySubjectAndDateAsync(dto.SubjectId, dto.Date);
        return _mapper.Map<IReadOnlyList<AttendanceDto>>(saved);
    }

    public async Task<IReadOnlyList<AttendanceDto>> GetBySubjectAndDateAsync(int subjectId, DateTime date)
    {
        var items = await _unitOfWork.Attendances.GetBySubjectAndDateAsync(subjectId, date);
        return _mapper.Map<IReadOnlyList<AttendanceDto>>(items);
    }

    public async Task<AttendanceSummaryDto> GetStudentSummaryAsync(int studentId)
    {
        var records = await _unitOfWork.Attendances.GetByStudentAsync(studentId);

        var total = records.Count;
        var present = records.Count(r => r.Status == AttendanceStatus.Present);
        var studentName = records.FirstOrDefault()?.Student?.FullName ?? string.Empty;

        return new AttendanceSummaryDto
        {
            StudentId = studentId,
            StudentName = studentName,
            TotalClasses = total,
            PresentCount = present,
            AttendancePercentage = total == 0 ? 0 : Math.Round((double)present / total * 100, 2)
        };
    }
}