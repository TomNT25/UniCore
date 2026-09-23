namespace UniCore.Helper.Constant
{
    public static class ExceptionConstants
    {
        public const string UnhandledExceptionUniCore = "An unhandled exception occurred: {Message}";
        public const string DefaultValidationFailedMessage = "Validation failed for the input request.";
        public const string DefaultUnexpectedErrorMessage = "An unexpected error occurred. Please try again later.";
        public const string MissingJwtSecretMessage = "Jwt:Secret configuration is missing in appsettings.json.";
    }
}