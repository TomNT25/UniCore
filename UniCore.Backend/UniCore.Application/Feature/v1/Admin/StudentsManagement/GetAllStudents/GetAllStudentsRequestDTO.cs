using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents
{
    public class GetAllStudentsRequestDTO : PageNumberPaginationRequest, IRequest<GetAllStudentsResponseDTO>
    {
    }
}
