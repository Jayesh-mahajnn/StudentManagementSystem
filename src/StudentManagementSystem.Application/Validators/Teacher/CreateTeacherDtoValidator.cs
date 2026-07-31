using FluentValidation;
using StudentManagementSystem.Application.DTOs.Teacher;

namespace StudentManagementSystem.Application.Validators.Teacher;

public class CreateTeacherDtoValidator : AbstractValidator<CreateTeacherDto>
{
    public CreateTeacherDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone must be exactly 10 digits.");
        RuleFor(x => x.Gender).NotEmpty().Must(g => new[] { "Male", "Female", "Other" }.Contains(g))
            .WithMessage("Gender must be Male, Female, or Other.");
        RuleFor(x => x.DateOfJoining).LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date of joining cannot be in the future.");
        RuleFor(x => x.DepartmentId).GreaterThan(0);
    }
}