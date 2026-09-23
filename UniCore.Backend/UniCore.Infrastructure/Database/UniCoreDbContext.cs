using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database.ModelCreating;

namespace UniCore.Infrastructure.Database
{
    public class UniCoreDbContext : DbContext
    {
        public UniCoreDbContext(DbContextOptions<UniCoreDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserPersonId> UserPersonIds { get; set; }
        public DbSet<UserMfaSetting> UserMfaSettings { get; set; }
        public DbSet<UserMfaBackupCode> UserMfaBackupCodes { get; set; }
        public DbSet<UserExternalLogin> UserExternalLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        public DbSet<UserAuthLog> UserAuthLogs { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassCourse> ClassCourses { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<WhitelistedEmail> WhitelistedEmails { get; set; }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementStudent> AnnouncementStudents { get; set; }
        public DbSet<AnnouncementEmailLog> AnnouncementEmailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserModelCreating.CreateModel(modelBuilder);
            UserProfileModelCreating.CreateModel(modelBuilder);
            UserPersonIdModelCreating.CreateModel(modelBuilder);
            UserMfaSettingModelCreating.CreateModel(modelBuilder);
            UserMfaBackupCodeModelCreating.CreateModel(modelBuilder);
            UserExternalLoginModelCreating.CreateModel(modelBuilder);
            RoleModelCreating.CreateModel(modelBuilder);
            PermissionModelCreating.CreateModel(modelBuilder);
            UserRoleModelCreating.CreateModel(modelBuilder);
            UserPermissionModelCreating.CreateModel(modelBuilder);
            RolePermissionModelCreating.CreateModel(modelBuilder);
            UserTokenModelCreating.CreateModel(modelBuilder);
            UserPasswordHistoryModelCreating.CreateModel(modelBuilder);
            UserAuthLogModelCreating.CreateModel(modelBuilder);
            AppLogModelCreating.CreateModel(modelBuilder);
            DepartmentModelCreating.CreateModel(modelBuilder);
            CourseModelCreating.CreateModel(modelBuilder);
            SchoolClassModelCreating.CreateModel(modelBuilder);
            CourseStudentModelCreating.CreateModel(modelBuilder);
            ScheduleModelCreating.CreateModel(modelBuilder);
            WhitelistedEmailModelCreating.CreateModel(modelBuilder);

            AnnouncementModelCreating.CreateModel(modelBuilder);
            AnnouncementStudentModelCreating.CreateModel(modelBuilder);
            AnnouncementEmailLogModelCreating.CreateModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}