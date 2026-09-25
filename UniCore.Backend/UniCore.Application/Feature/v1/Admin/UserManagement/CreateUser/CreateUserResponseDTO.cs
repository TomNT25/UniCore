using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserResponseDTO
    {
        [JsonPropertyName("items")]
        public CreateStudentAccountResultDTO Items { get; set; } = new();

        [JsonPropertyName("user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreateUserResultDTO? User { get; set; }
    }

    public class CreateStudentAccountResultDTO
    {
        [JsonPropertyName("success_accounts")]
        public List<StudentAccountSuccessItemDTO> SuccessAccounts { get; set; } = new();

        [JsonPropertyName("error_accounts")]
        public List<StudentAccountErrorItemDTO> ErrorAccounts { get; set; } = new();
    }

    public class StudentAccountSuccessItemDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class StudentAccountErrorItemDTO
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("error_type")]
        public string ErrorType { get; set; } = string.Empty;
    }
}
