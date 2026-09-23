using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileRequestDTO>
    {
        private static readonly string[] ValidGenders = { "MALE", "FEMALE", "OTHER" };

        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .MaximumLength(50).WithMessage("User ID cannot exceed 50 characters.");

            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.FullName), () =>
            {
                RuleFor(x => x.FullName)
                    .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
                    .Matches(@"^\+?[0-9\s\-()]{7,20}$").WithMessage("Invalid phone number format.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Gender), () =>
            {
                RuleFor(x => x.Gender)
                    .Must(g => ValidGenders.Contains(g!.Trim().ToUpperInvariant()))
                    .WithMessage("Gender must be MALE, FEMALE, or OTHER.");
            });

            When(x => x.BirthDate.HasValue, () =>
            {
                RuleFor(x => x.BirthDate!.Value)
                    .LessThan(DateTime.UtcNow).WithMessage("Birth date must be in the past.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
            {
                RuleFor(x => x.Address)
                    .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Bio), () =>
            {
                RuleFor(x => x.Bio)
                    .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.");
            });
        }
    }
}
