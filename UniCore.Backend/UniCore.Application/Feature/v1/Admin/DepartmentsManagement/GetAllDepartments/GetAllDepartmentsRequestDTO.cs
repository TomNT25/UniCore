using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllDepartments
{
    public class GetAllDepartmentsRequestDTO : PageNumberPaginationRequest, IRequest<GetAllDepartmentsResponseDTO>
    {
    }
}
