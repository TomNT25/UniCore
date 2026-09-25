using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.Contract.Util;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.ExceptionHandler;
using UniCore.Helper.Localization;
using UserEntity = UniCore.Application.Entity.User;
using RoleEntity = UniCore.Application.Entity.Role;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequestDTO, CreateUserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWhitelistedEmailRepository _whitelistedEmailRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IValidator<CreateUserRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IWhitelistedEmailRepository whitelistedEmailRepository,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IPasswordHasherService passwordHasherService,
            IValidator<CreateUserRequestDTO> validator,
            IJsonStringLocalizer localizer,
            ILogger<CreateUserHandler> logger)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _whitelistedEmailRepository = whitelistedEmailRepository;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _passwordHasherService = passwordHasherService;
            _validator = validator;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<CreateUserResponseDTO> HandleAsync(CreateUserRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var studentCode = (request.StudentCode)?.Trim()!;
            var email = request.Email.Trim();

            // 1. Check duplicate student code
            var existingByStudentCode = await _userRepository.GetByUsernameAsync(studentCode, cancellationToken);
            if (existingByStudentCode != null)
            {
                _logger.LogWarning("Duplicate student code: {StudentCode}", studentCode);
                throw new ConflictException(
                    "Duplicate student code",
                    new[] { $"Student code '{studentCode}' already exists" });
            }

            // 2. Check duplicate email
            var existingEmail = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (existingEmail != null)
            {
                _logger.LogWarning("Duplicate email: {Email}", email);
                throw new ConflictException(
                    "Duplicate email",
                    new[] { $"Email '{email}' already exists" });
            }

            // 3. Resolve Role (Student)
            RoleEntity? role = null;
            if (!string.IsNullOrWhiteSpace(request.RoleId))
            {
                role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            }

            if (role == null)
            {
                role = await _roleRepository.GetByIdAsync("role-student-003", cancellationToken);
            }

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

            // 4. Generate password & hash
            var rawPassword = !string.IsNullOrWhiteSpace(request.Password) ? request.Password : GenerateRandomPassword();
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
                IsActive = request.IsActive ?? true,
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

            // 5. Database transaction (Insert user & user_role, NO user_profiles as per STU-13)
            using (var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _userRepository.AddAsync(userEntity, cancellationToken);
                    await _userRoleRepository.AddAsync(userRole, cancellationToken);
                    await tx.CommitAsync(cancellationToken);
                }
                catch
                {
                    await tx.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            // 6. Send credentials email
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
                _logger.LogInformation("Credentials email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send credentials email to {Email}", email);
                throw new BadGatewayException(
                    "Failed to send credentials email",
                    new[] { "Email provider rejected the request" });
            }

            // 7. Update whitelisted_emails.student_id if email matches
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
                    _logger.LogInformation("Updated whitelisted_emails for student {UserId} ({Email})", userId, email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to update whitelisted_emails for {Email}", email);
            }

            // 8. Return response matching contract
            return new CreateUserResponseDTO
            {
                Items = new CreateStudentAccountResultDTO
                {
                    SuccessAccounts = new List<StudentAccountSuccessItemDTO>
                    {
                        new()
                        {
                            Id = userEntity.Id,
                            StudentCode = userEntity.StudentCode,
                            Email = userEntity.Email
                        }
                    },
                    ErrorAccounts = new List<StudentAccountErrorItemDTO>()
                },
                User = new CreateUserResultDTO
                {
                    Id = userEntity.Id,
                    StudentCode = userEntity.StudentCode,
                    Username = userEntity.Username,
                    Email = userEntity.Email,
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsActive = userEntity.IsActive,
                    CreatedAt = userEntity.CreatedAt
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
