using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Teacher;
using StudentManagementSystem.Shared.Exceptions;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;
using StudentManagementSystem.Application.Common.Models;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IValidator<CreateTeacherDto> _createValidator;
    private readonly IValidator<UpdateTeacherDto> _updateValidator;

    public TeachersController(
        ITeacherService teacherService,
        IValidator<CreateTeacherDto> createValidator,
        IValidator<UpdateTeacherDto> updateValidator)
    {
        _teacherService = teacherService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TeacherDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _teacherService.GetPagedAsync(paginationParams));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeacherDto>> GetById(int id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);
        if (teacher is null) throw new NotFoundException("Teacher", id);
        return Ok(teacher);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<TeacherDto>> Create(CreateTeacherDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw ToValidationException(validationResult);

        var created = await _teacherService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTeacherDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw ToValidationException(validationResult);

        var updated = await _teacherService.UpdateAsync(id, dto);
        if (!updated) throw new NotFoundException("Teacher", id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _teacherService.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("Teacher", id);
        return NoContent();
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}