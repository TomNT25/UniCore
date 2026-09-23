using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Identity.GetMyPersonId
{
    public class GetMyPersonIdHandler : IRequestHandler<GetMyPersonIdRequestDTO, GetMyPersonIdResponseDTO>
    {
        private readonly IUserPersonIdRepository _personIdRepository;
        private readonly IValidator<GetMyPersonIdRequestDTO> _validator;

        public GetMyPersonIdHandler(
            IUserPersonIdRepository personIdRepository,
            IValidator<GetMyPersonIdRequestDTO> validator)
        {
            _personIdRepository = personIdRepository;
            _validator = validator;
        }

        public async Task<GetMyPersonIdResponseDTO> HandleAsync(
            GetMyPersonIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _personIdRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (entity == null)
            {
                return new GetMyPersonIdResponseDTO
                {
                    HasRecord = false,
                    VerificationStatus = PersonIdConstants.Unverified
                };
            }

            return new GetMyPersonIdResponseDTO
            {
                HasRecord = true,
                Id = entity.Id,
                IdNumber = entity.IdNumber,
                FullName = entity.FullName,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Nationality = entity.Nationality,
                PlaceOfOrigin = entity.PlaceOfOrigin,
                PlaceOfResidence = entity.PlaceOfResidence,
                ExpireDate = entity.ExpireDate,
                VerificationStatus = entity.VerificationStatus,
                VerifiedAt = entity.VerifiedAt
            };
        }
    }
}
