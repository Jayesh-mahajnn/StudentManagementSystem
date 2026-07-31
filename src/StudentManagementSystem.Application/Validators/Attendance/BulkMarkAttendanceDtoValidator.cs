using FluentValidation;
using StudentManagementSystem.Application.DTOs.Attendance;

namespace StudentManagementSystem.Application.Validators.Attendance;

public class BulkMarkAttendanceDtoValidator : AbstractValidator<BulkMarkAttendanceDto>
{
    public BulkMarkAttendanceDtoValidator()
    {
        RuleFor(x => x.SubjectId).GreaterThan(0);
        RuleFor(x => x.TeacherId).GreaterThan(0);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Attendance date cannot be in the future.");
        RuleFor(x => x.Entries).NotEmpty().WithMessage("At least one attendance entry is required.");

        RuleForEach(x => x.Entries).ChildRules(entry =>
        {
            entry.RuleFor(e => e.StudentId).GreaterThan(0);
            entry.RuleFor(e => e.Status).NotEmpty()
                .Must(s => new[] { "Present", "Absent", "Late", "Excused" }.Contains(s))
                .WithMessage("Status must be Present, Absent, Late, or Excused.");
        });
    }
}