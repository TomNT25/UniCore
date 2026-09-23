/*
Post-Deployment Script UniCore
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\filepath.file
--------------------------------------------------------------------------------------
*/

PRINT 'Executing Post-Deployment Seed Scripts...';

:r .\SeedData\01_SeedRoles.sql
:r .\SeedData\02_SeedPermissions.sql
:r .\SeedData\03_SeedRolePermissions.sql
:r .\SeedData\04_SeedDepartments.sql
:r .\SeedData\05_SeedUsers.sql
:r .\SeedData\06_SeedUserPermissions.sql
:r .\SeedData\07_SeedCourses.sql
:r .\SeedData\08_SeedClasses.sql
:r .\SeedData\10_SeedCourseStudents.sql
:r .\SeedData\11_SeedSchedules.sql
:r .\SeedData\12_SeedWhitelistedEmails.sql
:r .\SeedData\13_SeedAnnouncements.sql
:r .\SeedData\14_SeedAnnouncementStudents.sql
:r .\SeedData\17_RedesignAnnouncementLifecycle.sql
:r .\SeedData\14_SeedAppLogs.sql
:r .\SeedData\18_CreateUserFaceProfiles.sql
:r .\SeedData\19_CreateFaceAuthLogs.sql
:r .\SeedData\20_AlterAnnouncementScopeValueLength.sql
:r .\SeedData\21_AddAnnouncementStudentIsSent.sql

PRINT 'Post-Deployment Seed Data complete.';
GO