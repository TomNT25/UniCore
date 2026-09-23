namespace UniCore.Application.Feature.v1.Admin.DepartmentsManagement.GetAllDepartments
{
    public class GetAllDepartmentsDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
