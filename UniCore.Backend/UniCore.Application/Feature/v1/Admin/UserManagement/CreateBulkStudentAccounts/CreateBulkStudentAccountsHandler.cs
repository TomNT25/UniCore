using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.Contract.Util;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;
using UserEntity = UniCore.Application.Entity.User;
using RoleEntity = UniCore.Application.Entity.Role;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateBulkStudentAccounts
{
    public class CreateBulkStudentAccountsHandler : IRequestHandler<CreateBulkStudentAccountsRequestDTO, CreateBulkStudentAccountsResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWhitelistedEmailRepository _whitelistedEmailRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ILogger<CreateBulkStudentAccountsHandler> _logger;

        public CreateBulkStudentAccountsHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IWhitelistedEmailRepository whitelistedEmailRepository,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IPasswordHasherService passwordHasherService,
            ILogger<CreateBulkStudentAccountsHandler> logger)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _whitelistedEmailRepository = whitelistedEmailRepository;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _passwordHasherService = passwordHasherService;
            _logger = logger;
        }

        public async Task<CreateBulkStudentAccountsResponseDTO> HandleAsync(
            CreateBulkStudentAccountsRequestDTO request,
            CancellationToken cancellationToken)
        {
            if (request.FileStream == null || request.FileStream.Length == 0)
            {
                throw new ValidationException(new[] { new ValidationFailure("file", "File must be .csv and not empty") });
            }

            using var reader = new StreamReader(request.FileStream, Encoding.UTF8);

            // 1. Read header line
            string? headerLine = await reader.ReadLineAsync(cancellationToken);
            while (headerLine != null && string.IsNullOrWhiteSpace(headerLine))
            {
                headerLine = await reader.ReadLineAsync(cancellationToken);
            }

            if (headerLine == null)
            {
                throw new ValidationException(new[] { new ValidationFailure("file", "CSV contains no data rows") });
            }

            var headers = headerLine.Split(',')
                .Select(h => h.Trim().ToLowerInvariant().Trim('"'))
                .ToList();

            int studentCodeIndex = headers.IndexOf("student_code");
            int emailIndex = headers.IndexOf("email");

            if (studentCodeIndex == -1 || emailIndex == -1)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure("file", "Missing required headers: student_code, email")
                });
            }

            // 2. Resolve Student Role
            var role = await _roleRepository.GetByIdAsync("role-student-003", cancellationToken);
            if (role == null)
            {
                var allRoles = await _roleRepository.GetAllAsync(cancellationToken);
                role = allRoles.FirstOrDefault(r => string.Equals(r.Name, "Student", StringComparison.OrdinalIgnoreCase))
                    ?? allRoles.FirstOrDefault(r => r.IsActive);
            }

            if (role == null)
            {
                throw new InvalidOperationException("Default Student role could not be resolved.");
            }

            var successAccounts = new List<StudentAccountSuccessItemDTO>();
            var errorAccounts = new List<StudentAccountErrorItemDTO>();

            var seenStudentCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            int dataRowCount = 0;
            string? line;

            // 3. Process each row
            while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                dataRowCount++;
                var columns = line.Split(',').Select(c => c.Trim().Trim('"')).ToList();

                var studentCode = studentCodeIndex < columns.Count ? columns[studentCodeIndex] : string.Empty;
                var email = emailIndex < columns.Count ? columns[emailIndex] : string.Empty;

                // Validate non-empty
                if (string.IsNullOrWhiteSpace(studentCode))
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "STUDENT_CODE"
                    });
                    continue;
                }

                if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "EMAIL"
                    });
                    continue;
                }

                // Check in-file duplicate
                if (!seenStudentCodes.Add(studentCode))
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "STUDENT_CODE"
                    });
                    continue;
                }

                if (!seenEmails.Add(email))
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "EMAIL"
                    });
                    continue;
                }

                // Check DB duplicate
                var existingStudentCode = await _userRepository.GetByUsernameAsync(studentCode, cancellationToken);
                if (existingStudentCode != null)
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "STUDENT_CODE"
                    });
                    continue;
                }

                var existingEmail = await _userRepository.GetByEmailAsync(email, cancellationToken);
                if (existingEmail != null)
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "EMAIL"
                    });
                    continue;
                }

                var whitelistedEmail = await _whitelistedEmailRepository.GetByEmailAsync(email, cancellationToken);
                if (whitelistedEmail == null)
                {
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "MAIL_NOT_EXIST"
                    });
                    continue;
                }

                // Create user
                var rawPassword = GenerateRandomPassword();
                var passwordHash = _passwordHasherService.HashPassword(rawPassword);
                var userId = Guid.NewGuid().ToString();
                var now = DateTime.UtcNow;

                var userEntity = new UserEntity
                {
                    Id = userId,
                    Code = null,
                    StudentCode = studentCode,
                    Username = studentCode,
                    Email = email,
                    PasswordHash = passwordHash,
                    Provider = "SYSTEM",
                    FailedLoginAttempts = 0,
                    LockoutEnd = null,
                    IsActive = true,
                    IsEmailVerified = false,
                    EmailVerifiedAt = null,
                    LastLoginAt = null,
                    CreatedAt = now,
                    CreatedBy = request.AdminUserId
                };

                var userRole = new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    RoleId = role.Id,
                    AssignedAt = now,
                    IsActive = true,
                    Role = role
                };

                // Per-row transaction
                try
                {
                    using var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                    await _userRepository.AddAsync(userEntity, cancellationToken);
                    await _userRoleRepository.AddAsync(userRole, cancellationToken);
                    await tx.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to insert user {StudentCode} during bulk creation", studentCode);
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = null,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "STUDENT_CODE"
                    });
                    continue;
                }

                // Dispatch credentials email
                bool emailSent = false;
                try
                {
                    var emailSubject = "UniCore - Thông tin tài khoản sinh viên";
                    var emailBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 8px;'>
                        <h2 style='color: #2563eb;'>UniCore student account information</h2>
                        <p>Hello,</p>
                        <p>Your UniCore learning account has been successfully created by the Administrator.</p>
                        <p>Here is your login information:</p>
                        <div style='background-color: #f1f5f9; padding: 15px; border-radius: 8px; font-size: 16px; color: #1e293b; margin: 20px 0;'>
                            <p style='margin: 5px 0;'><strong>Username (Username / Student Code):</strong> {studentCode}</p>
                            <p style='margin: 5px 0;'><strong>Temporary Password:</strong> {rawPassword}</p>
                        </div>
                        <p>Please log in to the system and complete the personal information verification (ID card)..</p>
                        <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
                        <p style='color: #64748b; font-size: 12px;'>This is an automated email; please do not reply to it.</p>
                    </div>";

                    await _emailSender.SendAsync(email, emailSubject, emailBody, cancellationToken);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send credentials email to {Email}", email);
                }

                if (!emailSent)
                {
                    // Row was inserted, but mail failed -> SENT_EMAIL_FAILED with user id
                    errorAccounts.Add(new StudentAccountErrorItemDTO
                    {
                        Id = userEntity.Id,
                        StudentCode = studentCode,
                        Email = email,
                        ErrorType = "SENT_EMAIL_FAILED"
                    });
                    continue;
                }

                // If email succeeded, update whitelist
                try
                {
                    var whitelisted = await _whitelistedEmailRepository.GetByEmailAsync(email, cancellationToken);
                    if (whitelisted != null)
                    {
                        whitelisted.StudentId = userEntity.Id;
                        whitelisted.UpdatedAt = DateTime.UtcNow;
                        whitelisted.UpdatedBy = request.AdminUserId;
                        await _whitelistedEmailRepository.UpdateAsync(whitelisted, cancellationToken);
                        await _whitelistedEmailRepository.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to update whitelisted_emails for {Email}", email);
                }

                successAccounts.Add(new StudentAccountSuccessItemDTO
                {
                    Id = userEntity.Id,
                    StudentCode = studentCode,
                    Email = email
                });
            }

            if (dataRowCount == 0)
            {
                throw new ValidationException(new[] { new ValidationFailure("file", "CSV contains no data rows") });
            }

            return new CreateBulkStudentAccountsResponseDTO
            {
                Items = new CreateStudentAccountResultDTO
                {
                    SuccessAccounts = successAccounts,
                    ErrorAccounts = errorAccounts
                }
            };
        }

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%";
            var bytes = new byte[10];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var sb = new StringBuilder(10);
            foreach (var b in bytes)
            {
                sb.Append(chars[b % chars.Length]);
            }
            return sb.ToString();
        }
    }
}
