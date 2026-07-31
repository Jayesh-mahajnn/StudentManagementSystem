using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Marks;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MarksController : ControllerBase
{
    private readonly IMarksService _service;
    private readonly IValidator<BulkCreateMarksDto> _bulkValidator;

    public MarksController(IMarksService service, IValidator<BulkCreateMarksDto> bulkValidator)
    {
        _service = service;
        _bulkValidator = bulkValidator;
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost("bulk")]
    public async Task<ActionResult<IReadOnlyList<MarksDto>>> CreateBulk(BulkCreateMarksDto dto)
    {
        var result = await _bulkValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var saved = await _service.CreateBulkMarksAsync(dto);
        return Ok(saved);
    }

    [HttpGet("student/{studentId:int}")]
    public async Task<ActionResult<IReadOnlyList<MarksDto>>> GetByStudent(int studentId)
    {
        return Ok(await _service.GetByStudentAsync(studentId));
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}