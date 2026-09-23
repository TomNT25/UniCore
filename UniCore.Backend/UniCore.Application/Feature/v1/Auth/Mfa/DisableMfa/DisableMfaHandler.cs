using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Auth.Mfa.DisableMfa
{
    public class DisableMfaHandler : IRequestHandler<DisableMfaRequestDTO, DisableMfaResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IValidator<DisableMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public DisableMfaHandler(
            IUserRepository userRepository,
            IUserMfaSettingRepository mfaSettingRepository,
            IPasswordHasherService passwordHasherService,
            IValidator<DisableMfaRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _mfaSettingRepository = mfaSettingRepository;
            _passwordHasherService = passwordHasherService;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<DisableMfaResponseDTO> HandleAsync(DisableMfaRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (userEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                throw new KeyNotFoundException(msg);
            }

            var isPasswordValid = _passwordHasherService.VerifyHashedPassword(userEntity.PasswordHash, request.Password);
            if (!isPasswordValid)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidCurrentPassword);
                throw new ValidationException(new[] { new ValidationFailure("Password", msg) });
            }

            var mfaSetting = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (mfaSetting != null)
            {
                mfaSetting.IsMfaEnabled = false;
                mfaSetting.UpdatedAt = DateTime.UtcNow;
                await _mfaSettingRepository.UpdateAsync(mfaSetting, cancellationToken);
            }

            var successMsg = _localizer.GetString(MessageConstants.Auth.MfaDisableSuccess);
            return new DisableMfaResponseDTO
            {
                Success = true,
                Message = successMsg
            };
        }
    }
}
