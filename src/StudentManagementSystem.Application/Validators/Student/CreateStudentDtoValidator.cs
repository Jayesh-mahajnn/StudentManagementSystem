using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Student;

namespace StudentManagementSystem.Application.Validators.Student;

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStudentDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone must be exactly 10 digits.");
        RuleFor(x => x.Gender).NotEmpty().Must(g => new[] { "Male", "Female", "Other" }.Contains(g))
            .WithMessage("Gender must be Male, Female, or Other.");
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow.AddYears(-15))
            .WithMessage("Student must be at least 15 years old.");
        RuleFor(x => x.EnrollmentNumber).NotEmpty().MaximumLength(30);
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);

        // Cross-field business rule: Course must belong to the given Department
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