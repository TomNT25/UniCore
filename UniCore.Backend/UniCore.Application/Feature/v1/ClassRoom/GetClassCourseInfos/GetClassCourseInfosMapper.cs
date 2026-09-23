using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class ClassCourseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Course, GetClassCourseInfosDTO>()
                .Map(dest => dest.CourseName, src => src.Name ?? string.Empty)
                .Map(dest => dest.CourseDescriptions, src => src.Description ?? string.Empty)
                .Map(dest => dest.CourseLecturers, src => src.Description ?? string.Empty);
        }
    }
}
