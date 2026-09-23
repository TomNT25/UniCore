using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Auth.Mfa.EnableMfa
{
    public class EnableMfaHandler : IRequestHandler<EnableMfaRequestDTO, EnableMfaResponseDTO>
    {
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IValidator<EnableMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public EnableMfaHandler(
            IUserMfaSettingRepository mfaSettingRepository,
            IValidator<EnableMfaRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _mfaSettingRepository = mfaSettingRepository;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<EnableMfaResponseDTO> HandleAsync(EnableMfaRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var mfaSetting = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (mfaSetting == null || string.IsNullOrWhiteSpace(mfaSetting.SecretKey))
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidMfaCode);
                throw new ValidationException(new[] { new ValidationFailure("Code", msg) });
            }

            if (string.IsNullOrWhiteSpace(request.Code) || request.Code.Length < 6)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidMfaCode);
                throw new ValidationException(new[] { new ValidationFailure("Code", msg) });
            }

            mfaSetting.IsMfaEnabled = true;
            mfaSetting.EnabledAt = DateTime.UtcNow;
            mfaSetting.UpdatedAt = DateTime.UtcNow;

            await _mfaSettingRepository.UpdateAsync(mfaSetting, cancellationToken);

            var successMsg = _localizer.GetString(MessageConstants.Auth.MfaEnableSuccess);
            return new EnableMfaResponseDTO
            {
                Success = true,
                Message = successMsg
            };
        }
    }
}
