using FluentValidation;
using StudentManagementSystem.Application.DTOs.Timetable;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Validators.Timetable;

public class UpdateTimetableDtoValidator : AbstractValidator<UpdateTimetableDto>
{
    public UpdateTimetableDtoValidator()
    {
        RuleFor(x => x.DayOfWeek).NotEmpty()
            .Must(d => Enum.TryParse<DayOfWeekEnum>(d, true, out _))
            .WithMessage("DayOfWeek must be Monday through Saturday.");

        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
        RuleFor(x => x.TeacherId).GreaterThan(0);
    }
}