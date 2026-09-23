
namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllDepartments
{
    public class GetAllDepartmentsDTO
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
