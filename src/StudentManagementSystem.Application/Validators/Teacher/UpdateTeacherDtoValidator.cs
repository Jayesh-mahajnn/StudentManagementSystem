using FluentValidation;
using StudentManagementSystem.Application.DTOs.Teacher;

namespace StudentManagementSystem.Application.Validators.Teacher;

public class UpdateTeacherDtoValidator : AbstractValidator<UpdateTeacherDto>
{
    public UpdateTeacherDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone must be exactly 10 digits.");
        RuleFor(x => x.DepartmentId).GreaterThan(0);
    }
}