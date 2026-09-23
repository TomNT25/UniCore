using FluentValidation;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdValidator : AbstractValidator<GetAnnouncementByIdRequestDTO>
    {
        public GetAnnouncementByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Announcement ID is required.");
        }
    }
}
