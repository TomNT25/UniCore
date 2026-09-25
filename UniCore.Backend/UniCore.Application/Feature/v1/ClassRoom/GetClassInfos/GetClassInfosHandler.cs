using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoHandler : IRequestHandler<GetClassInfoRequestDTO, GetClassInfoResponseDTO>
    {
        private readonly ISchoolClassRepository _schoolClassRepo;

        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetClassInfoRequestDTO> _validator;

        public GetClassInfoHandler(
            ISchoolClassRepository schoolClassRepo,
            IUserRepository userRepository,
            IValidator<GetClassInfoRequestDTO> validator
            )
        {
            _validator = validator;
            _userRepository = userRepository;
            _schoolClassRepo = schoolClassRepo;
        }

        public async Task<GetClassInfoResponseDTO> HandleAsync(GetClassInfoRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserID, ct);

            if(user?.ClassId is null)
            {
                throw new NullReferenceException(nameof(user));
            }

            var classID = user.ClassId;

            var classInfos = await _schoolClassRepo.GetInfoByIdAsync(classID, ct);

            if (classInfos is null)
            {
                throw new NullReferenceException(nameof(classInfos));
            }

            return classInfos.Adapt<GetClassInfoResponseDTO>();

        }

    }
}
