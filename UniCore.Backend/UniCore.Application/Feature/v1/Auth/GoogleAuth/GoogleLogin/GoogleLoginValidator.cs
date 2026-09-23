using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.GoogleLogin
{
    public class GoogleLoginValidator : AbstractValidator<GoogleLoginRequestDTO>
    {
        public GoogleLoginValidator()
        {
            RuleFor(x => x.IdToken).NotEmpty().WithMessage("Google ID token is required.");
        }
    }
}
