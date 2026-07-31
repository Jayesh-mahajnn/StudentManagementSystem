using FluentValidation;
using StudentManagementSystem.Application.DTOs.Department;

namespace StudentManagementSystem.Application.Validators.Department;

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(10).WithMessage("Department code must not exceed 10 characters.")
            .Matches("^[A-Z]+$").WithMessage("Department code must contain only uppercase letters.");
    }
}