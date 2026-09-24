using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetAllCourses;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse;

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

            config.NewConfig<Course, CourseDetailDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);

            config.NewConfig<Course, CreateCourseResultDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);

            config.NewConfig<Course, UpdateCourseResultDTO>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);
        }
    }
}
