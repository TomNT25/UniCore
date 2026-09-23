using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class CourseStudentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CourseStudent, GetCoursesStudentsResponseDTO>()
                .Map(dest => dest.CourseStartDate, src => src.StartDate.ToShortDateString() ?? string.Empty)
                .Map(dest => dest.CourseEndDate, src => src.EndDate.ToShortDateString() ?? string.Empty)
                .Map(dest => dest.CourseStatus, src => src.Status ?? string.Empty)
                .Map(dest => dest.CourseName, src => src.Status ?? string.Empty)
                .Map(dest => dest.CourseDescription, src => src.Status ?? string.Empty)
                .Map(dest => dest.CourseFinalScore, src => src.FinalScore ?? -1);
        }
    }
}
