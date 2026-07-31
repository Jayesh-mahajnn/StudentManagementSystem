using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Announcement;
using StudentManagementSystem.Shared.Exceptions;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _service;
    private readonly IValidator<CreateAnnouncementDto> _createValidator;
    private readonly IValidator<UpdateAnnouncementDto> _updateValidator;

    public AnnouncementsController(IAnnouncementService service, IValidator<CreateAnnouncementDto> createValidator, IValidator<UpdateAnnouncementDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AnnouncementDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnnouncementDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) throw new NotFoundException("Announcement", id);
        return Ok(item);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    public async Task<ActionResult<AnnouncementDto>> Create(CreateAnnouncementDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

        var created = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateAnnouncementDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid) throw ToValidationException(result);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) throw new NotFoundException("Announcement", id);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("Announcement", id);
        return NoContent();
    }

    private static ValidationException ToValidationException(FluentValidation.Results.ValidationResult result)
    {
        var errors = result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return new ValidationException(errors);
    }
}