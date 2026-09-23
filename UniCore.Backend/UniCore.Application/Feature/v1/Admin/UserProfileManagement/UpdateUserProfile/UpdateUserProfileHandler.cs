using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequestDTO, UpdateUserProfileResponseDTO>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserProfileRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateUserProfileHandler(
            IUserProfileRepository userProfileRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UpdateUserProfileRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userProfileRepository = userProfileRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateUserProfileResponseDTO> HandleAsync(UpdateUserProfileRequestDTO request, CancellationToken cancellationToken)
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
            if (profile == null)
            {
                var calcFullName = request.FullName;
                if (string.IsNullOrWhiteSpace(calcFullName))
                {
                    calcFullName = string.Join(" ", new[] { request.FirstName, request.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                }

                profile = new UserProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    FullName = string.IsNullOrWhiteSpace(calcFullName) ? null : calcFullName,
                    PhoneNumber = request.PhoneNumber,
                    AvatarUrl = request.AvatarUrl,
                    AvatarMediaFileId = request.AvatarMediaFileId,
                    Gender = request.Gender,
                    BirthDate = request.BirthDate,
                    Address = request.Address,
                    Bio = request.Bio,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _userProfileRepository.AddAsync(profile, cancellationToken);
            }
            else
            {
                var calcFullName = request.FullName;
                if (string.IsNullOrWhiteSpace(calcFullName))
                {
                    calcFullName = string.Join(" ", new[] { request.FirstName, request.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                }

                profile.FirstName = request.FirstName;
                profile.LastName = request.LastName;
                profile.FullName = string.IsNullOrWhiteSpace(calcFullName) ? null : calcFullName;
                profile.PhoneNumber = request.PhoneNumber;
                profile.AvatarUrl = request.AvatarUrl;
                profile.AvatarMediaFileId = request.AvatarMediaFileId;
                profile.Gender = request.Gender;
                profile.BirthDate = request.BirthDate;
                profile.Address = request.Address;
                profile.Bio = request.Bio;
                profile.UpdatedAt = DateTime.UtcNow;

                await _userProfileRepository.UpdateAsync(profile, cancellationToken);
            }

            return new UpdateUserProfileResponseDTO
            {
                Profile = _mapper.Map<UserProfileDTO>(profile)
            };
        }
    }
}
