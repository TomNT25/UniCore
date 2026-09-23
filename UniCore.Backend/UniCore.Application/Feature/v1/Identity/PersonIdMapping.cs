using System.Globalization;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Identity
{
    public static class PersonIdMapping
    {
        private static readonly string[] DateFormats =
        {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "yyyy-MM-dd",
            "dd-MM-yyyy"
        };

        public static DateTime? ParseOcrDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (DateTime.TryParseExact(
                    value.Trim(),
                    DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var exact))
            {
                return exact.Date;
            }

            return DateTime.TryParse(value.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)
                ? parsed.Date
                : null;
        }

        public static UniCore.Application.Entity.UserPersonId ToVerifiedEntity(
            string userId,
            string idNumber,
            string fullName,
            string? dateOfBirth,
            string? sex,
            string? nationality,
            string? placeOfOrigin,
            string? placeOfResidence,
            string? dateOfExpiry)
        {
            var now = DateTime.UtcNow;
            return new UniCore.Application.Entity.UserPersonId
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                IdNumber = idNumber.Trim(),
                FullName = fullName.Trim(),
                CardType = PersonIdConstants.CardTypeCccdChip,
                BirthDate = ParseOcrDate(dateOfBirth),
                Gender = string.IsNullOrWhiteSpace(sex) ? null : sex.Trim(),
                Nationality = string.IsNullOrWhiteSpace(nationality) ? "Việt Nam" : nationality.Trim(),
                PlaceOfOrigin = string.IsNullOrWhiteSpace(placeOfOrigin) ? null : placeOfOrigin.Trim(),
                PlaceOfResidence = string.IsNullOrWhiteSpace(placeOfResidence) ? null : placeOfResidence.Trim(),
                ExpireDate = ParseOcrDate(dateOfExpiry),
                VerificationStatus = PersonIdConstants.Verified,
                VerifiedAt = now,
                VerifiedBy = PersonIdConstants.VerifiedByOcr,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = userId,
                UpdatedBy = userId
            };
        }
    }
}
