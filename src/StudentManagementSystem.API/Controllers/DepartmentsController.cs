using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Common.Models;
using StudentManagementSystem.Application.DTOs.Department;
using StudentManagementSystem.Shared.Exceptions;
using ValidationException = StudentManagementSystem.Shared.Exceptions.ValidationException;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Any authenticated user can access GET endpoints
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IValidator<CreateDepartmentDto> _createValidator;
    private readonly IValidator<UpdateDepartmentDto> _updateValidator;

    public DepartmentsController(
        IDepartmentService departmentService,
        IValidator<CreateDepartmentDto> createValidator,
        IValidator<UpdateDepartmentDto> updateValidator)
    {
        _departmentService = departmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET: api/departments
    [HttpGet]
    public async Task<ActionResult<PagedResult<DepartmentDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _departmentService.GetPagedAsync(paginationParams));
    }

    // GET: api/departments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentDto>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department is null)
            throw new NotFoundException("Department", id);

        return Ok(department);
    }

    // POST: api/departments
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            throw new ValidationException(errors);
        }

        var created = await _departmentService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    // PUT: api/departments/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDepartmentDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            throw new ValidationException(errors);
        }

        var updated = await _departmentService.UpdateAsync(id, dto);

        if (!updated)
            throw new NotFoundException("Department", id);

        return NoContent();
    }

    // DELETE: api/departments/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);

        if (!deleted)
            throw new NotFoundException("Department", id);

        return NoContent();
    }
}