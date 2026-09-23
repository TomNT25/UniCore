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
        private readonly IValidator<GetClassInfoRequestDTO> _validator;

        public GetClassInfoHandler(
            ISchoolClassRepository schoolClassRepo,
            IValidator<GetClassInfoRequestDTO> validator
            )
        {
            _validator = validator;
            _schoolClassRepo = schoolClassRepo;
        }

        public async Task<GetClassInfoResponseDTO> HandleAsync(GetClassInfoRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }


            var classInfos = await _schoolClassRepo.GetInfoByIdAsync(request.ClassID, ct);

            if (classInfos is null)
            {
                throw new NullReferenceException(nameof(classInfos));
            }

            return classInfos.Adapt<GetClassInfoResponseDTO>();

        }

    }
}
