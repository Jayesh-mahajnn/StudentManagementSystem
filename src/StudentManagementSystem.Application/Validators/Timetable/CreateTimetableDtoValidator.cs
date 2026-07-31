using FluentValidation;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Timetable;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Validators.Timetable;

public class CreateTimetableDtoValidator : AbstractValidator<CreateTimetableDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTimetableDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.DayOfWeek).NotEmpty()
            .Must(d => Enum.TryParse<DayOfWeekEnum>(d, true, out _))
            .WithMessage("DayOfWeek must be Monday through Saturday.");

        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be after StartTime.");

        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.SubjectId).GreaterThan(0);
        RuleFor(x => x.TeacherId).GreaterThan(0);

        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                if (!Enum.TryParse<DayOfWeekEnum>(dto.DayOfWeek, true, out var day)) return true; // already caught above
                var taken = await _unitOfWork.Timetables.IsSlotTakenAsync(dto.TeacherId, day, dto.StartTime);
                return !taken;
            })
            .WithMessage("This teacher is already booked for the selected day and time slot.")
            .WithName("TeacherId");
    }
}