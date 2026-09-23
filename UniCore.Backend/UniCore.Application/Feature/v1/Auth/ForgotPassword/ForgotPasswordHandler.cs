using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;

namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequestDTO, ForgotPasswordResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public ForgotPasswordHandler(IUserRepository userRepository, IOtpService otpService)
        {
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public async Task<ForgotPasswordResponseDTO> HandleAsync(ForgotPasswordRequestDTO request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("No user found with the provided email address.");
            }

            var otpCode = _otpService.GenerateOtp($"forgot_password_{request.Email}");

            return new ForgotPasswordResponseDTO
            {
                Email = request.Email,
                OtpCode = otpCode,
                Message = "Password reset OTP generated successfully"
            };
        }
    }
}
