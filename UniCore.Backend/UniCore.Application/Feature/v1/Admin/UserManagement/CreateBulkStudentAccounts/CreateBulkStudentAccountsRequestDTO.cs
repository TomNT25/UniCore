using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateBulkStudentAccounts
{
    public class CreateBulkStudentAccountsRequestDTO : IRequest<CreateBulkStudentAccountsResponseDTO>
    {
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string? AdminUserId { get; set; }
    }
}
