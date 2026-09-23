using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus
{
    public class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusRequestDTO, UpdateUserStatusResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserStatusRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateUserStatusHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UpdateUserStatusRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateUserStatusResponseDTO> HandleAsync(UpdateUserStatusRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (userEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.UserNotFound);
                throw new ValidationException(new[] { new ValidationFailure("Id", msg) });
            }

            userEntity.IsActive = request.IsActive;
            userEntity.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(userEntity, cancellationToken);

            return new UpdateUserStatusResponseDTO
            {
                User = _mapper.Map<UserDTO>(userEntity)
            };
        }
    }
}
