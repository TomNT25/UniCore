using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.RefreshToken
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDTO>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
