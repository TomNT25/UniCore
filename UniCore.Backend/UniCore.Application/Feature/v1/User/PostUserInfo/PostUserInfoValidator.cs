using FluentValidation;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public class PutUserInfoValidator : AbstractValidator<PostUserInfoRequestDTO>
    {
        public PutUserInfoValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

   
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");


            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^0[0-9]{9}$").WithMessage("Invalid Vietnamese phone number format (must be 10 digits starting with 0).");


            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be 'Male', 'Female', or 'Other'.");


            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.");


            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");


            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.");
        }
    }
}
