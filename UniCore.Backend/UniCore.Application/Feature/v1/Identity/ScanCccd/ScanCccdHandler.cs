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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IValidator<ScanCccdRequestDTO> _validator;

        public ScanCccdHandler(
            IAiOcrClient aiOcrClient,
            IUserPersonIdRepository personIdRepository,
            IUserProfileRepository userProfileRepository,
            IValidator<ScanCccdRequestDTO> validator)
        {
            _aiOcrClient = aiOcrClient;
            _personIdRepository = personIdRepository;
            _userProfileRepository = userProfileRepository;
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

            // Upsert UserProfile with is_verified = false
            var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var birthDate = PersonIdMapping.ParseOcrDate(scan.DateOfBirth);
            var (firstName, lastName) = SplitFullName(scan.FullName);
            var now = DateTime.UtcNow;

            if (profile == null)
            {
                profile = new UniCore.Application.Entity.UserProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    FullName = scan.FullName.Trim(),
                    FirstName = firstName,
                    LastName = lastName,
                    Gender = scan.Sex?.Trim(),
                    BirthDate = birthDate,
                    Address = scan.PlaceOfResidence?.Trim() ?? scan.PlaceOfOrigin?.Trim(),
                    IsActive = true,
                    IsVerified = false,
                    VerifiedAt = null,
                    VerifiedBy = null,
                    CreatedAt = now,
                    CreatedBy = request.UserId
                };

                await _userProfileRepository.AddAsync(profile, cancellationToken);
            }
            else
            {
                profile.FullName = scan.FullName.Trim();
                profile.FirstName = firstName;
                profile.LastName = lastName;
                profile.Gender = scan.Sex?.Trim() ?? profile.Gender;
                profile.BirthDate = birthDate ?? profile.BirthDate;
                profile.Address = scan.PlaceOfResidence?.Trim() ?? scan.PlaceOfOrigin?.Trim() ?? profile.Address;
                profile.IsVerified = false; // Must be verified by Admin
                profile.VerifiedAt = null;
                profile.VerifiedBy = null;
                profile.UpdatedAt = now;
                profile.UpdatedBy = request.UserId;

                await _userProfileRepository.UpdateAsync(profile, cancellationToken);
            }

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
                PersonIdRecordId = saved.Id,
                IsProfileVerified = profile.IsVerified,
                UserProfileId = profile.Id
            };
        }

        private static (string? FirstName, string? LastName) SplitFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return (null, null);
            }

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return (parts[0], parts[0]);
            }

            var lastName = parts[0];
            var firstName = string.Join(" ", parts.Skip(1));
            return (firstName, lastName);
        }
    }
}
