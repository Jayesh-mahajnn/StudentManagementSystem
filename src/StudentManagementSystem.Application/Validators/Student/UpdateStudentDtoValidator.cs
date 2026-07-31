using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Student;

namespace StudentManagementSystem.Application.Validators.Student;

public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone must be exactly 10 digits.");
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);

        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
                return course is not null && course.DepartmentId == dto.DepartmentId;
            })
            .WithMessage("The selected course does not belong to the selected department.")
            .WithName("CourseId");
    }
}