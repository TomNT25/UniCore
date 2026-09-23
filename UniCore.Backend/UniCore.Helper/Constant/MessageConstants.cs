namespace UniCore.Helper.Constant
{
    public static class MessageConstants
    {
        public static class Announcement
        {
            public const string GetAllSuccess = "Announcement.GetAllSuccess";
            public const string GetByIdSuccess = "Announcement.GetByIdSuccess";
            public const string CreateSuccess = "Announcement.CreateSuccess";
            public const string UpdateSuccess = "Announcement.UpdateSuccess";
            public const string DeleteSuccess = "Announcement.DeleteSuccess";
            public const string PreviewSuccess = "Announcement.PreviewSuccess";
            public const string DeliveryReportSuccess = "Announcement.DeliveryReportSuccess";
            public const string GetPublicListSuccess = "Announcement.GetPublicListSuccess";
            public const string GetPublicByIdSuccess = "Announcement.GetPublicByIdSuccess";
            public const string NotFound = "Announcement.NotFound";
        }

        public static class Identity
        {
            public const string ScanCccdSuccess = "Identity.ScanCccdSuccess";
            public const string GetCccdSuccess = "Identity.GetCccdSuccess";
            public const string ImageRequired = "Identity.ImageRequired";
            public const string ImageQualityRejected = "Identity.ImageQualityRejected";
            public const string IdNumberAlreadyUsed = "Identity.IdNumberAlreadyUsed";
        }

        public static class FaceAuth
        {
            // Success messages
            public const string EnrollSuccess = "FaceAuth.EnrollSuccess";
            public const string SetPinSuccess = "FaceAuth.SetPinSuccess";
            public const string GetStatusSuccess = "FaceAuth.GetStatusSuccess";
            public const string LoginSuccess = "FaceAuth.LoginSuccess";
            public const string VerifyPinSuccess = "FaceAuth.VerifyPinSuccess";

            // Error messages
            public const string EnrollFailed = "FaceAuth.EnrollFailed";
            public const string SetPinFailed = "FaceAuth.SetPinFailed";
            public const string LoginFailed = "FaceAuth.LoginFailed";
            public const string VerifyPinFailed = "FaceAuth.VerifyPinFailed";

            // Validation messages
            public const string UserIdRequired = "FaceAuth.UserIdRequired";
            public const string ImagesRequired = "FaceAuth.ImagesRequired";
            public const string InvalidImageCount = "FaceAuth.InvalidImageCount";
            public const string InvalidImageType = "FaceAuth.InvalidImageType";
            public const string ImageTooLarge = "FaceAuth.ImageTooLarge";
            public const string PinRequired = "FaceAuth.PinRequired";
            public const string PinInvalidLength = "FaceAuth.PinInvalidLength";
            public const string PinMustBeDigits = "FaceAuth.PinMustBeDigits";
            public const string ConfirmPinRequired = "FaceAuth.ConfirmPinRequired";
            public const string PinMismatch = "FaceAuth.PinMismatch";
            public const string InvalidPin = "FaceAuth.InvalidPin";
            public const string PinLocked = "FaceAuth.PinLocked";

            // Status messages
            public const string NotEnrolled = "FaceAuth.NotEnrolled";
            public const string AlreadyEnrolled = "FaceAuth.AlreadyEnrolled";
            public const string AccountSuspended = "FaceAuth.AccountSuspended";
            public const string NoMatch = "FaceAuth.NoMatch";
            public const string ChallengeExpired = "FaceAuth.ChallengeExpired";
            public const string InvalidChallenge = "FaceAuth.InvalidChallenge";
        }

        public static class System
        {
            public const string ValidationFailed = "System.ValidationFailed";
            public const string UnexpectedError = "System.UnexpectedError";
            public const string ResourceNotFound = "System.ResourceNotFound";
            public const string UnauthorizedAccess = "System.UnauthorizedAccess";
            public const string ForbiddenAccess = "System.ForbiddenAccess";
        }


        public static class Auth

        {

            public const string LoginSuccess = "Auth.LoginSuccess";

            public const string RegisterSuccess = "Auth.RegisterSuccess";

            public const string LogoutSuccess = "Auth.LogoutSuccess";

            public const string RefreshTokenSuccess = "Auth.RefreshTokenSuccess";

            public const string SendOtpSuccess = "Auth.SendOtpSuccess";

            public const string VerifyOtpSuccess = "Auth.VerifyOtpSuccess";

            public const string ForgotPasswordSuccess = "Auth.ForgotPasswordSuccess";

            public const string ResetPasswordSuccess = "Auth.ResetPasswordSuccess";

            public const string GetMeSuccess = "Auth.GetMeSuccess";

            public const string ChangePasswordSuccess = "Auth.ChangePasswordSuccess";

            public const string MfaSetupSuccess = "Auth.MfaSetupSuccess";

            public const string MfaEnableSuccess = "Auth.MfaEnableSuccess";

            public const string MfaDisableSuccess = "Auth.MfaDisableSuccess";

            public const string MfaVerifySuccess = "Auth.MfaVerifySuccess";

            public const string GoogleLoginSuccess = "Auth.GoogleLoginSuccess";

            public const string SimulateGoogleTokenSuccess = "Auth.SimulateGoogleTokenSuccess";

            public const string InvalidCredentials = "Auth.InvalidCredentials";

            public const string InvalidCurrentPassword = "Auth.InvalidCurrentPassword";

            public const string InvalidRefreshToken = "Auth.InvalidRefreshToken";

            public const string EmailRegistered = "Auth.EmailRegistered";

            public const string UsernameTaken = "Auth.UsernameTaken";

            public const string DefaultRoleNotFound = "Auth.DefaultRoleNotFound";

            public const string InvalidOrExpiredOtp = "Auth.InvalidOrExpiredOtp";

            public const string UserNotFound = "Auth.UserNotFound";

            public const string IdentityNotFound = "Auth.IdentityNotFound";

            public const string InvalidMfaCode = "Auth.InvalidMfaCode";

            public const string MfaRequired = "Auth.MfaRequired";

            public const string InvalidGoogleToken = "Auth.InvalidGoogleToken";

        }

        public static class Student

        {

            public const string GetAllSuccess = "Student.GetAllSuccess";

            public const string GetByIdSuccess = "Student.GetByIdSuccess";

            public const string AddSuccess = "Student.AddSuccess";

            public const string UpdateSuccess = "Student.UpdateSuccess";

            public const string DeleteSuccess = "Student.DeleteSuccess";

        }

        public static class Role

        {

            public const string GetAllSuccess = "Role.GetAllSuccess";

            public const string GetByIdSuccess = "Role.GetByIdSuccess";

            public const string CreateSuccess = "Role.CreateSuccess";

        public const string UpdateSuccess = "Role.UpdateSuccess";

            public const string DeleteSuccess = "Role.DeleteSuccess";

            public const string GrantPermissionSuccess = "Role.GrantPermissionSuccess";

            public const string RevokePermissionSuccess = "Role.RevokePermissionSuccess";

            public const string GrantUserSuccess = "Role.GrantUserSuccess";

            public const string RevokeUserSuccess = "Role.RevokeUserSuccess";

            public const string NotFound = "Role.NotFound";

            public const string CodeAlreadyExists = "Role.CodeAlreadyExists";

        }

        public static class Permission

        {

            public const string GetAllSuccess = "Permission.GetAllSuccess";

            public const string GetByIdSuccess = "Permission.GetByIdSuccess";

            public const string CreateSuccess = "Permission.CreateSuccess";

            public const string UpdateSuccess = "Permission.UpdateSuccess";

            public const string DeleteSuccess = "Permission.DeleteSuccess";

            public const string GrantUserSuccess = "Permission.GrantUserSuccess";

            public const string RevokeUserSuccess = "Permission.RevokeUserSuccess";

            public const string NotFound = "Permission.NotFound";

            public const string CodeAlreadyExists = "Permission.CodeAlreadyExists";

        }

        public static class Admin

        {

            public const string GetDashboardSuccess = "Admin.GetDashboardSuccess";

            public const string GetAllUsersSuccess = "Admin.GetAllUsersSuccess";

            public const string GetUserByIdSuccess = "Admin.GetUserByIdSuccess";

            public const string CreateUserSuccess = "Admin.CreateUserSuccess";

            public const string UpdateUserSuccess = "Admin.UpdateUserSuccess";

            public const string DeleteUserSuccess = "Admin.DeleteUserSuccess";

            public const string UpdateUserStatusSuccess = "Admin.UpdateUserStatusSuccess";

            public const string GetUserProfileSuccess = "Admin.GetUserProfileSuccess";

            public const string UpdateUserProfileSuccess = "Admin.UpdateUserProfileSuccess";

            public const string UserNotFound = "Admin.UserNotFound";

            public const string ProfileNotFound = "Admin.ProfileNotFound";

            public const string EmailAlreadyExists = "Admin.EmailAlreadyExists";

            public const string UsernameAlreadyExists = "Admin.UsernameAlreadyExists";

            public const string GetAllStudentsSuccess = "Admin.GetAllStudentsSuccess";

            public const string GetAllCoursesSuccess = "Admin.GetAllCoursesSuccess";

            public const string GetCourseByIdSuccess = "Admin.GetCourseByIdSuccess";

            public const string CreateCourseSuccess = "Admin.CreateCourseSuccess";

            public const string UpdateCourseSuccess = "Admin.UpdateCourseSuccess";

            public const string DeleteCourseSuccess = "Admin.DeleteCourseSuccess";

    public const string UpdateCourseStatusSuccess = "Admin.UpdateCourseStatusSuccess";

            public const string CourseNotFound = "Admin.CourseNotFound";

            public const string CourseCodeAlreadyExists = "Admin.CourseCodeAlreadyExists";

            public const string DepartmentNotFound = "Admin.DepartmentNotFound";

            public const string GetAllAuditLogsSuccess = "Admin.GetAllAuditLogsSuccess";

            public const string GetAuditLogByIdSuccess = "Admin.GetAuditLogByIdSuccess";

            public const string CreateAuditLogSuccess = "Admin.CreateAuditLogSuccess";

            public const string UpdateAuditLogSuccess = "Admin.UpdateAuditLogSuccess";

            public const string DeleteAuditLogSuccess = "Admin.DeleteAuditLogSuccess";

            public const string AuditLogNotFound = "Admin.AuditLogNotFound";

        }

        public static class Rbac

        {

            public const string GetOverviewSuccess = "Rbac.GetOverviewSuccess";

            public const string GetUserAccessMatrixSuccess = "Rbac.GetUserAccessMatrixSuccess";

            public const string GrantUserRoleSuccess = "Rbac.GrantUserRoleSuccess";

            public const string RevokeUserRoleSuccess = "Rbac.RevokeUserRoleSuccess";

            public const string GrantUserPermissionSuccess = "Rbac.GrantUserPermissionSuccess";

            public const string RevokeUserPermissionSuccess = "Rbac.RevokeUserPermissionSuccess";

            public const string GrantRolePermissionSuccess = "Rbac.GrantRolePermissionSuccess";

            public const string RevokeRolePermissionSuccess = "Rbac.RevokeRolePermissionSuccess";

            public const string UserNotFound = "Rbac.UserNotFound";

            public const string RoleNotFound = "Rbac.RoleNotFound";

            public const string PermissionNotFound = "Rbac.PermissionNotFound";

        }
    }
}
 