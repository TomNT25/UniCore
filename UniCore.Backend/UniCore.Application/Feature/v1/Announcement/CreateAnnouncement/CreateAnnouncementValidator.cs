using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.CreateAnnouncement
{
    public class CreateAnnouncementValidator : AbstractValidator<CreateAnnouncementRequestDTO>
    {
        public CreateAnnouncementValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(255)
                .WithMessage("Title cannot exceed 255 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Type is required.")
                .Must(t => AnnouncementConstants.Type.Allowed.Contains(t))
                .WithMessage("Type must be NORMAL, IMPORTANT, or URGENT.");

            RuleFor(x => x.ScopeType)
                .NotEmpty()
                .WithMessage("ScopeType is required.")
                .Must(s => AnnouncementConstants.Scope.Supported.Contains(s))
                .WithMessage("ScopeType must be PUBLIC, STUDENTS, DEPARTMENT, CLASS, COURSE, or SPECIFIC_STUDENTS.");

            RuleFor(x => x.PublishDate)
                .NotEmpty()
                .WithMessage("PublishDate is required.");

            RuleFor(x => x.ExpiredDate)
                .NotEmpty()
                .WithMessage("ExpiredDate is required.")
                .GreaterThan(x => x.PublishDate)
                .WithMessage("ExpiredDate must be greater than PublishDate.");
        }
    }
}
