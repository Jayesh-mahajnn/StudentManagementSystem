using FluentValidation;
using StudentManagementSystem.Application.DTOs.Announcement;

namespace StudentManagementSystem.Application.Validators.Announcement;

public class CreateAnnouncementDtoValidator : AbstractValidator<CreateAnnouncementDto>
{
    public CreateAnnouncementDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
    }
}