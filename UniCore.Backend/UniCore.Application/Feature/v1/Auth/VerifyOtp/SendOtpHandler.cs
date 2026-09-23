using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class SendOtpHandler : IRequestHandler<SendOtpRequestDTO, SendOtpResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public SendOtpHandler(IUserRepository userRepository, IOtpService otpService)
        {
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public async Task<SendOtpResponseDTO> HandleAsync(SendOtpRequestDTO request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("No user found with the provided email address.");
            }

            var otpCode = _otpService.GenerateOtp($"verify_email_{request.Email}");

            return new SendOtpResponseDTO
            {
                Email = request.Email,
                OtpCode = otpCode,
                Message = "OTP sent successfully to email"
            };
        }
    }
}
