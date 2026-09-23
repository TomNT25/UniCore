using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.MapperProfile
{
    public class CourseMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Course, CourseDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);

            config.NewConfig<Course, Feature.v1.Admin.CoursesManagement.GetAllCourses.GetAllCoursesDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);
        }
    }
}
