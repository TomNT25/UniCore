using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class GetClassCourseInfosRequestDTO : IRequest<IEnumerable<GetClassCourseInfosResponseDTO>>
    {
        public IEnumerable<string> CourseIds { get; set; }
    }
}
