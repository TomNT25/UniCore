namespace UniCore.Application.Contract.Util
{
    public interface IOtpService
    {
        string GenerateOtp(string key, TimeSpan? expiration = null);
        bool VerifyOtp(string key, string otp);
        void InvalidateOtp(string key);

        Task<string> GenerateOtpAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<bool> VerifyOtpAsync(string key, string otp, CancellationToken cancellationToken = default);
        Task InvalidateOtpAsync(string key, CancellationToken cancellationToken = default);
    }
}
