using FluentValidation;
using StudentManagementSystem.Application.DTOs.Marks;

namespace StudentManagementSystem.Application.Validators.Marks;

public class BulkCreateMarksDtoValidator : AbstractValidator<BulkCreateMarksDto>
{
    public BulkCreateMarksDtoValidator()
    {
        RuleFor(x => x.SubjectId).GreaterThan(0);
        RuleFor(x => x.TeacherId).GreaterThan(0);
        RuleFor(x => x.MaxMarks).GreaterThan(0);
        RuleFor(x => x.ExamType).NotEmpty()
            .Must(t => new[] { "Quiz", "Midterm", "Final", "Assignment" }.Contains(t))
            .WithMessage("ExamType must be Quiz, Midterm, Final, or Assignment.");
        RuleFor(x => x.Entries).NotEmpty();

        RuleForEach(x => x.Entries).ChildRules(entry =>
        {
            entry.RuleFor(e => e.StudentId).GreaterThan(0);
            entry.RuleFor(e => e.ObtainedMarks).GreaterThanOrEqualTo(0);
        });

        RuleFor(x => x)
            .Must(dto => dto.Entries.All(e => e.ObtainedMarks <= dto.MaxMarks))
            .WithMessage("ObtainedMarks cannot exceed MaxMarks for any entry.")
            .WithName("Entries");
    }
}