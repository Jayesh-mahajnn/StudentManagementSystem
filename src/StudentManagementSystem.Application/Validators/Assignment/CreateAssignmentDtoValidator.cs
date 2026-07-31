using FluentValidation;
using StudentManagementSystem.Application.DTOs.Assignment;

namespace StudentManagementSystem.Application.Validators.Assignment;

public class CreateAssignmentDtoValidator : AbstractValidator<CreateAssignmentDto>
{
    public CreateAssignmentDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DueDate).GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
        RuleFor(x => x.SubjectId).GreaterThan(0);
        RuleFor(x => x.TeacherId).GreaterThan(0);
    }
}