using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById
{
    public class GetPublicAnnouncementByIdHandler
        : IRequestHandler<GetPublicAnnouncementByIdRequestDTO, GetPublicAnnouncementByIdResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IValidator<GetPublicAnnouncementByIdRequestDTO> _validator;

        public GetPublicAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IValidator<GetPublicAnnouncementByIdRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _validator = validator;
        }

        public async Task<GetPublicAnnouncementByIdResponseDTO> HandleAsync(
            GetPublicAnnouncementByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetPublishedPublicByIdAsync(request.Id, cancellationToken);

            var utcNow = DateTime.UtcNow;

            return new GetPublicAnnouncementByIdResponseDTO
            {
                Announcement = entity == null
                    ? null
                    : AdminAnnouncementMapping.ToAdminItem(entity, utcNow, includeContent: true)
            };
        }
    }

    public class GetPublicAnnouncementByIdValidator : AbstractValidator<GetPublicAnnouncementByIdRequestDTO>
    {
        public GetPublicAnnouncementByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Announcement ID is required.");
        }
    }
}
