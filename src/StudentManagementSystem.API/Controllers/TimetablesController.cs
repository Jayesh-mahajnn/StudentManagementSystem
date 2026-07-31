using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Timetable;
using StudentManagementSystem.Shared.Exceptions;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimetablesController : ControllerBase
{
    private readonly ITimetableService _service;
    private readonly IValidator<CreateTimetableDto> _createValidator;
    private readonly IValidator<UpdateTimetableDto> _updateValidator;

    public TimetablesController(ITimetableService service, IValidator<CreateTimetableDto> createValidator, IValidator<UpdateTimetableDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TimetableDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TimetableDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) throw new NotFoundException("Timetable entry", id);
        return Ok(item);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<TimetableDto>> Create(CreateTimetableDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTimetableDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) throw new NotFoundException("Timetable entry", id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("Timetable entry", id);
        return NoContent();
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}