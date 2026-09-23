using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.SimulateGoogleToken
{
    public class SimulateGoogleTokenValidator : AbstractValidator<SimulateGoogleTokenRequestDTO>
    {
        public SimulateGoogleTokenValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
