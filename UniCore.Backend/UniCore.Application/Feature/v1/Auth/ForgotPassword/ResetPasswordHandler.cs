using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequestDTO, ResetPasswordResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IOtpService _otpService;

        public ResetPasswordHandler(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService,
            IOtpService otpService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _otpService = otpService;
        }

        public async Task<ResetPasswordResponseDTO> HandleAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken)
        {
            var isValid = _otpService.VerifyOtp($"forgot_password_{request.Email}", request.OtpCode);
            if (!isValid)
            {
                throw new InvalidOperationException(MessageConstants.Auth.InvalidOrExpiredOtp);
            }

            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException(MessageConstants.Auth.UserNotFound);
            }

            user.PasswordHash = _passwordHasherService.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new ResetPasswordResponseDTO
            {
                Email = request.Email,
                Success = true,
                Message = MessageConstants.Auth.ResetPasswordSuccess
            };
        }
    }
}
