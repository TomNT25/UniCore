using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus
{
    public class UpdateCourseStatusRequestDTO : IRequest<UpdateCourseStatusResponseDTO>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
