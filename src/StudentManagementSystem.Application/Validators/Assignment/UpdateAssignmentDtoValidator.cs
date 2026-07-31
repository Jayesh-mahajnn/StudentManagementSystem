using FluentValidation;
using StudentManagementSystem.Application.DTOs.Assignment;

namespace StudentManagementSystem.Application.Validators.Assignment;

public class UpdateAssignmentDtoValidator : AbstractValidator<UpdateAssignmentDto>
{
    public UpdateAssignmentDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DueDate).GreaterThan(DateTime.UtcNow);
    }
}