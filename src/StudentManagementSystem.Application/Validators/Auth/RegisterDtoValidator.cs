using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Auth;

namespace StudentManagementSystem.Application.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.");
        RuleFor(x => x.Role).NotEmpty()
            .Must(r => new[] { "Admin", "Teacher", "Student" }.Contains(r))
            .WithMessage("Role must be Admin, Teacher, or Student.");

        // Student-specific: enrollment number required and must match an existing, unlinked Student
        When(x => x.Role == "Student", () =>
        {
            RuleFor(x => x.EnrollmentNumber)
                .NotEmpty().WithMessage("EnrollmentNumber is required when registering as a Student.");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(dto.EnrollmentNumber)) return true; // already caught above
                    var student = await _unitOfWork.Students.GetByEnrollmentNumberAsync(dto.EnrollmentNumber);
                    return student is not null;
                })
                .WithMessage("No student record found with this enrollment number. Contact your administrator.")
                .WithName("EnrollmentNumber");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(dto.EnrollmentNumber)) return true;
                    var student = await _unitOfWork.Students.GetByEnrollmentNumberAsync(dto.EnrollmentNumber);
                    if (student is null) return true; // already caught above
                    var existingUser = await _unitOfWork.Users.FindAsync(u => u.StudentId == student.Id);
                    return !existingUser.Any();
                })
                .WithMessage("This student record is already linked to a login account.")
                .WithName("EnrollmentNumber");
        });

        // Teacher-specific: email must match an existing, unlinked Teacher
        When(x => x.Role == "Teacher", () =>
        {
            RuleFor(x => x.TeacherEmail)
                .NotEmpty().WithMessage("TeacherEmail is required when registering as a Teacher.");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(dto.TeacherEmail)) return true;
                    var teacher = await _unitOfWork.Teachers.GetByEmailAsync(dto.TeacherEmail);
                    return teacher is not null;
                })
                .WithMessage("No teacher record found with this email. Contact your administrator.")
                .WithName("TeacherEmail");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(dto.TeacherEmail)) return true;
                    var teacher = await _unitOfWork.Teachers.GetByEmailAsync(dto.TeacherEmail);
                    if (teacher is null) return true;
                    var existingUser = await _unitOfWork.Users.FindAsync(u => u.TeacherId == teacher.Id);
                    return !existingUser.Any();
                })
                .WithMessage("This teacher record is already linked to a login account.")
                .WithName("TeacherEmail");
        });
    }
}