using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpRequestDTO, VerifyOtpResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public VerifyOtpHandler(IUserRepository userRepository, IOtpService otpService)
        {
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public async Task<VerifyOtpResponseDTO> HandleAsync(VerifyOtpRequestDTO request, CancellationToken cancellationToken)
        {
            var isValid = _otpService.VerifyOtp($"verify_email_{request.Email}", request.OtpCode);
            if (!isValid)
            {
                throw new InvalidOperationException(MessageConstants.Auth.InvalidOrExpiredOtp);
            }

            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException(MessageConstants.Auth.UserNotFound);
            }

            user.IsEmailVerified = true;
            user.EmailVerifiedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new VerifyOtpResponseDTO
            {
                Email = request.Email,
                IsVerified = true,
                Message = MessageConstants.Auth.VerifyOtpSuccess
            };
        }
    }
}
