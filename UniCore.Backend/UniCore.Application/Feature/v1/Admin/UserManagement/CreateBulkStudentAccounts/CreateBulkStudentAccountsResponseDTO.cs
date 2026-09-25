using System.Text.Json.Serialization;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateBulkStudentAccounts
{
    public class CreateBulkStudentAccountsResponseDTO
    {
        [JsonPropertyName("items")]
        public CreateStudentAccountResultDTO Items { get; set; } = new();
    }
}
