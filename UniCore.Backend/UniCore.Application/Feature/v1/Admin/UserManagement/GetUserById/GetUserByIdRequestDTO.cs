using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById
{
    public class GetUserByIdRequestDTO : IRequest<GetUserByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
