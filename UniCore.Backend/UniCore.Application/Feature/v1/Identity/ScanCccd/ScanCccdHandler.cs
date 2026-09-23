using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Identity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Identity.ScanCccd
{
    public class ScanCccdHandler : IRequestHandler<ScanCccdRequestDTO, ScanCccdResponseDTO>
    {
        private readonly IAiOcrClient _aiOcrClient;
        private readonly IUserPersonIdRepository _personIdRepository;
        private readonly IValidator<ScanCccdRequestDTO> _validator;

        public ScanCccdHandler(
            IAiOcrClient aiOcrClient,
            IUserPersonIdRepository personIdRepository,
            IValidator<ScanCccdRequestDTO> validator)
        {
            _aiOcrClient = aiOcrClient;
            _personIdRepository = personIdRepository;
            _validator = validator;
        }

        public async Task<ScanCccdResponseDTO> HandleAsync(ScanCccdRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var scan = await _aiOcrClient.ScanAsync(
                request.UserId,
                request.ImageStream!,
                request.FileName,
                request.ContentType,
                cancellationToken);

            var existingByNumber = await _personIdRepository.GetByIdNumberAsync(scan.IdNumber, cancellationToken);
            if (existingByNumber != null
                && !string.Equals(existingByNumber.UserId, request.UserId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "This CCCD id_number is already linked to another account.");
            }

            var entity = PersonIdMapping.ToVerifiedEntity(
                request.UserId,
                scan.IdNumber,
                scan.FullName,
                scan.DateOfBirth,
                scan.Sex,
                scan.Nationality,
                scan.PlaceOfOrigin,
                scan.PlaceOfResidence,
                scan.DateOfExpiry);

            var saved = await _personIdRepository.UpsertFromOcrAsync(entity, cancellationToken);

            return new ScanCccdResponseDTO
            {
                IdNumber = saved.IdNumber,
                FullName = saved.FullName,
                DateOfBirth = scan.DateOfBirth,
                Sex = saved.Gender,
                Nationality = saved.Nationality,
                PlaceOfOrigin = saved.PlaceOfOrigin,
                PlaceOfResidence = saved.PlaceOfResidence,
                DateOfExpiry = scan.DateOfExpiry,
                VerificationStatus = saved.VerificationStatus,
                VerifiedAt = saved.VerifiedAt,
                PersonIdRecordId = saved.Id
            };
        }
    }
}
