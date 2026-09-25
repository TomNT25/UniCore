using System.Security.Cryptography;
using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Auth.Mfa.EnableMfa
{
    public class EnableMfaHandler : IRequestHandler<EnableMfaRequestDTO, EnableMfaResponseDTO>
    {
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IUserMfaBackupCodeRepository _mfaBackupCodeRepository;
        private readonly IValidator<EnableMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public EnableMfaHandler(
            IUserMfaSettingRepository mfaSettingRepository,
            IPasswordHasherService passwordHasherService,
            IUserMfaBackupCodeRepository mfaBackupCodeRepository,
            IValidator<EnableMfaRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _mfaSettingRepository = mfaSettingRepository;
            _passwordHasherService = passwordHasherService;
            _mfaBackupCodeRepository = mfaBackupCodeRepository;
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

            // Generate secret key (base32 string)
            var secretBytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretBytes);
            }
            var secretKey = Base32Encode(secretBytes);

            var mfaSetting = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (mfaSetting == null || string.IsNullOrWhiteSpace(mfaSetting.SecretKey))
            {
                mfaSetting = new UserMfaSetting
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    MfaMethod = "OTP",
                    SecretKey = secretKey,
                    IsMfaEnabled = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _mfaSettingRepository.AddAsync(mfaSetting, cancellationToken);
                
                // Generate 10 Backup Codes
                var plainBackupCodes = new List<string>();
                var existingCodes = await _mfaBackupCodeRepository.GetByUserIdAsync(request.UserId, cancellationToken);
                if (existingCodes.Count > 0)
                {
                    _mfaBackupCodeRepository.DeleteRange(existingCodes);
                }

                var newBackupEntities = new List<UserMfaBackupCode>();
                for (int i = 0; i < 10; i++)
                {
                    var rawCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                    plainBackupCodes.Add(rawCode);

                    newBackupEntities.Add(new UserMfaBackupCode
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = request.UserId,
                        CodeHash = _passwordHasherService.HashPassword(rawCode),
                        IsUsed = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await _mfaBackupCodeRepository.AddRangeAsync(newBackupEntities, cancellationToken);
            }
            else
            {
                mfaSetting.SecretKey = secretKey;
                mfaSetting.UpdatedAt = DateTime.UtcNow;
                await _mfaSettingRepository.UpdateAsync(mfaSetting, cancellationToken);
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

        private static string Base32Encode(byte[] data)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var result = new System.Text.StringBuilder((data.Length * 8 + 4) / 5);
            int buffer = data[0];
            int next = 1;
            int bitsLeft = 8;

            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < 5)
                {
                    if (next < data.Length)
                    {
                        buffer = (buffer << 8) | (data[next++] & 0xff);
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = 5 - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                int index = 0x1f & (buffer >> (bitsLeft - 5));
                bitsLeft -= 5;
                result.Append(chars[index]);
            }
            return result.ToString();
        }
    }
}
