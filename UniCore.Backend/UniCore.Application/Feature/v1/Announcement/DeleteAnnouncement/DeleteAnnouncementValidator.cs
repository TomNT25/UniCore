using FluentValidation;

namespace UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement
{
    public class DeleteAnnouncementValidator : AbstractValidator<DeleteAnnouncementRequestDTO>
    {
        public DeleteAnnouncementValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Announcement ID is required.");
        }
    }
}
