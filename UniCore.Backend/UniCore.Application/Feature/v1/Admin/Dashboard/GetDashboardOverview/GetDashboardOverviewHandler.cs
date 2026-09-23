using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview
{
    public class GetDashboardOverviewHandler : IRequestHandler<GetDashboardOverviewRequestDTO, GetDashboardOverviewResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetDashboardOverviewRequestDTO> _validator;

        public GetDashboardOverviewHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IDepartmentRepository departmentRepository,
            ICourseRepository courseRepository,
            IMapper mapper,
            IValidator<GetDashboardOverviewRequestDTO> validator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetDashboardOverviewResponseDTO> HandleAsync(GetDashboardOverviewRequestDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var totalUsers = await _userRepository.CountAsync(cancellationToken: cancellationToken);
            var activeUsers = await _userRepository.CountAsync(u => u.IsActive, cancellationToken);
            var inactiveUsers = await _userRepository.CountAsync(u => !u.IsActive, cancellationToken);
            var verifiedUsers = await _userRepository.CountAsync(u => u.IsEmailVerified, cancellationToken);

            var totalRoles = await _roleRepository.CountAsync(cancellationToken: cancellationToken);
            var totalDepartments = await _departmentRepository.CountAsync(cancellationToken: cancellationToken);
            var totalCourses = await _courseRepository.CountAsync(cancellationToken: cancellationToken);

            var recentUsersPaged = await _userRepository.GetPageNumberPaginationAsync<UserDTO>(
                new PageNumberPaginationRequest
                {
                    PageNumber = 1,
                    PageSize = 5,
                    SortColumn = "CreatedAt",
                    SortDescending = true
                },
                filter: null,
                cancellationToken: cancellationToken);

            return new GetDashboardOverviewResponseDTO
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                VerifiedUsers = verifiedUsers,
                TotalRoles = totalRoles,
                TotalDepartments = totalDepartments,
                TotalCourses = totalCourses,
                RecentUsers = recentUsersPaged.Items.ToList()
            };
        }
    }
}
