using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Auth.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequestDTO, ChangePasswordResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IValidator<ChangePasswordRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public ChangePasswordHandler(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService,
            IValidator<ChangePasswordRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<ChangePasswordResponseDTO> HandleAsync(ChangePasswordRequestDTO request, CancellationToken cancellationToken)
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

            var isCurrentValid = _passwordHasherService.VerifyHashedPassword(userEntity.PasswordHash, request.CurrentPassword);
            if (!isCurrentValid)
            {
                var msg = _localizer.GetString(MessageConstants.Auth.InvalidCurrentPassword);
                throw new ValidationException(new[] { new ValidationFailure("CurrentPassword", msg) });
            }

            userEntity.PasswordHash = _passwordHasherService.HashPassword(request.NewPassword);
            userEntity.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(userEntity, cancellationToken);

            var successMsg = _localizer.GetString(MessageConstants.Auth.ChangePasswordSuccess);
            return new ChangePasswordResponseDTO
            {
                Success = true,
                Message = successMsg
            };
        }
    }
}
