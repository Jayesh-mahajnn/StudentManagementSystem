using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Assignment;
using StudentManagementSystem.Shared.Exceptions;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _service;
    private readonly IValidator<CreateAssignmentDto> _createValidator;
    private readonly IValidator<UpdateAssignmentDto> _updateValidator;

    public AssignmentsController(IAssignmentService service, IValidator<CreateAssignmentDto> createValidator, IValidator<UpdateAssignmentDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssignmentDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AssignmentDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) throw new NotFoundException("Assignment", id);
        return Ok(item);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    public async Task<ActionResult<AssignmentDto>> Create(CreateAssignmentDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateAssignmentDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) throw new NotFoundException("Assignment", id);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("Assignment", id);
        return NoContent();
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}