using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class GetClassCourseInfosRequestDTO : IRequest<GetClassCourseInfosResponseDTO>
    {
        public string UserID { get; set; }
    }
}
