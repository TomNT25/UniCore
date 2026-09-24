using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.VerifyUserProfile
{
    public class VerifyUserProfileHandler : IRequestHandler<VerifyUserProfileRequestDTO, VerifyUserProfileResponseDTO>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<VerifyUserProfileRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public VerifyUserProfileHandler(
            IUserProfileRepository userProfileRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<VerifyUserProfileRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userProfileRepository = userProfileRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<VerifyUserProfileResponseDTO> HandleAsync(VerifyUserProfileRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (userEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.UserNotFound);
                throw new ValidationException(new[] { new ValidationFailure("UserId", msg) });
            }

            var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var now = DateTime.UtcNow;

            if (profile == null)
            {
                profile = new UserProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    IsActive = true,
                    IsVerified = request.IsVerified,
                    VerifiedAt = request.IsVerified ? now : null,
                    VerifiedBy = request.IsVerified ? request.AdminUserId : null,
                    CreatedAt = now,
                    CreatedBy = request.AdminUserId
                };

                await _userProfileRepository.AddAsync(profile, cancellationToken);
            }
            else
            {
                profile.IsVerified = request.IsVerified;
                profile.VerifiedAt = request.IsVerified ? now : null;
                profile.VerifiedBy = request.IsVerified ? request.AdminUserId : null;
                profile.UpdatedAt = now;
                profile.UpdatedBy = request.AdminUserId;

                await _userProfileRepository.UpdateAsync(profile, cancellationToken);
            }

            return new VerifyUserProfileResponseDTO
            {
                UserId = request.UserId,
                IsVerified = profile.IsVerified,
                VerifiedAt = profile.VerifiedAt,
                VerifiedBy = profile.VerifiedBy,
                Profile = _mapper.Map<UserProfileDetailDTO>(profile)
            };
        }
    }
}
