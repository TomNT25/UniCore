using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse
{
    public class UpdateCourseRequestDTO : IRequest<UpdateCourseResponseDTO>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DepartmentId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
