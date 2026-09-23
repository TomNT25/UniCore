using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllCourses;

namespace UniCore.Application.MapperProfile
{
    public class CourseMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Course, CourseDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);

            config.NewConfig<Course, GetAllCoursesDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);
        }
    }
}
