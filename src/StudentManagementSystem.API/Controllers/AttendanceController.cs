using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Attendance;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;
    private readonly IValidator<BulkMarkAttendanceDto> _bulkValidator;

    public AttendanceController(IAttendanceService service, IValidator<BulkMarkAttendanceDto> bulkValidator)
    {
        _service = service;
        _bulkValidator = bulkValidator;
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost("bulk")]
    public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> MarkBulk(BulkMarkAttendanceDto dto)
    {
        var result = await _bulkValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var saved = await _service.MarkBulkAttendanceAsync(dto);
        return Ok(saved);
    }

    [HttpGet("subject/{subjectId:int}/date/{date:datetime}")]
    public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> GetBySubjectAndDate(int subjectId, DateTime date)
    {
        return Ok(await _service.GetBySubjectAndDateAsync(subjectId, date));
    }

    [HttpGet("student/{studentId:int}/summary")]
    public async Task<ActionResult<AttendanceSummaryDto>> GetStudentSummary(int studentId)
    {
        return Ok(await _service.GetStudentSummaryAsync(studentId));
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}