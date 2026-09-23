using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequestDTO, DeleteUserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IValidator<DeleteUserRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public DeleteUserHandler(
            IUserRepository userRepository,
            IUserProfileRepository userProfileRepository,
            IValidator<DeleteUserRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<DeleteUserResponseDTO> HandleAsync(DeleteUserRequestDTO request, CancellationToken cancellationToken)
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

            var profile = await _userProfileRepository.GetByUserIdAsync(userEntity.Id, cancellationToken);
            if (profile != null)
            {
                await _userProfileRepository.DeleteAsync(profile, cancellationToken);
            }

            var success = await _userRepository.DeleteAsync(userEntity, cancellationToken);
            return new DeleteUserResponseDTO
            {
                Success = success
            };
        }
    }
}
