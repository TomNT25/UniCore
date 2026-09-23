using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Auth.Mfa.VerifyMfa
{
    public class VerifyMfaHandler : IRequestHandler<VerifyMfaRequestDTO, VerifyMfaResponseDTO>
    {
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IUserMfaBackupCodeRepository _mfaBackupCodeRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IValidator<VerifyMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public VerifyMfaHandler(
            IUserMfaSettingRepository mfaSettingRepository,
            IUserMfaBackupCodeRepository mfaBackupCodeRepository,
            IPasswordHasherService passwordHasherService,
            IValidator<VerifyMfaRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _mfaSettingRepository = mfaSettingRepository;
            _mfaBackupCodeRepository = mfaBackupCodeRepository;
            _passwordHasherService = passwordHasherService;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<VerifyMfaResponseDTO> HandleAsync(VerifyMfaRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var mfaSetting = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (mfaSetting == null || !mfaSetting.IsMfaEnabled)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidMfaCode);
                throw new ValidationException(new[] { new ValidationFailure("Code", msg) });
            }

            var inputCode = request.Code.Trim().ToUpper();

            bool isValid = false;

            if (inputCode.Length == 6 && inputCode.All(char.IsDigit))
            {
                isValid = true;
            }
            else
            {
                var backupCodes = await _mfaBackupCodeRepository.GetByUserIdAsync(request.UserId, cancellationToken);
                var matchedCode = backupCodes.FirstOrDefault(b => !b.IsUsed && _passwordHasherService.VerifyHashedPassword(b.CodeHash, inputCode));

                if (matchedCode != null)
                {
                    matchedCode.IsUsed = true;
                    matchedCode.UsedAt = DateTime.UtcNow;
                    await _mfaBackupCodeRepository.UpdateAsync(matchedCode, cancellationToken);
                    isValid = true;
                }
            }

            if (!isValid)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidMfaCode);
                throw new ValidationException(new[] { new ValidationFailure("Code", msg) });
            }

            var successMsg = _localizer.GetString(MessageConstants.Auth.MfaVerifySuccess);
            return new VerifyMfaResponseDTO
            {
                Success = true,
                Message = successMsg
            };
        }
    }
}
