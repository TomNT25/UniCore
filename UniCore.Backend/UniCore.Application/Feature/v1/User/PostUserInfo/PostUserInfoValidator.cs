using FluentValidation;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public class PutUserInfoValidator : AbstractValidator<PostUserInfoRequestDTO>
    {
        public PutUserInfoValidator()
        {

            RuleFor(x => x.PostUserBio.firstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

   
            RuleFor(x => x.PostUserBio.lastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.PostUserBio.fullName)
                .NotEmpty().WithMessage("Full name is required.");


            RuleFor(x => x.PostUserBio.phoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^0[0-9]{9}$").WithMessage("Invalid Vietnamese phone number format (must be 10 digits starting with 0).");


            RuleFor(x => x.PostUserBio.gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be 'Male', 'Female', or 'Other'.");


            RuleFor(x => x.PostUserBio.birthDate)
                .NotEmpty().WithMessage("Birth date is required.");


            RuleFor(x => x.PostUserBio.address)
                .NotEmpty().WithMessage("Address is required.");


            RuleFor(x => x.PostUserBio.bio)
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.");
        }
    }
}
