namespace UniCore.Application.Entity
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string? StudentCode { get; set; }
        public string? ClassId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Provider { get; set; } = "system";
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual SchoolClass? Class { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
        public virtual UserProfile? UserProfile { get; set; }
        public virtual UserPersonId? UserPersonId { get; set; }
        public virtual UserMfaSetting? UserMfaSetting { get; set; }
        public virtual ICollection<UserMfaBackupCode> UserMfaBackupCodes { get; set; } = new List<UserMfaBackupCode>();
        public virtual ICollection<UserExternalLogin> UserExternalLogins { get; set; } = new List<UserExternalLogin>();
        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public virtual ICollection<UserPasswordHistory> UserPasswordHistories { get; set; } = new List<UserPasswordHistory>();
        public virtual ICollection<UserAuthLog> UserAuthLogs { get; set; } = new List<UserAuthLog>();
        public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public virtual ICollection<WhitelistedEmail> WhitelistedEmails { get; set; } = new List<WhitelistedEmail>();
        public virtual ICollection<AnnouncementStudent> AnnouncementStudents { get; set; } = new List<AnnouncementStudent>();
    }
}
