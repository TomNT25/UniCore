using Mapster;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CourseStudent, GetMyCoursesDTO>
                .NewConfig()
                .Map(
                    dest => dest.Courses,
                    src => src.Course
                )
                .Map(
                    dest => dest.StudentCourses,
                    src => src
                );

            TypeAdapterConfig<Course, GetCourseDTO>
                .NewConfig();

            TypeAdapterConfig<Department, GetDepartmentsDTO>
                .NewConfig();

            TypeAdapterConfig<CourseStudent, GetStudentCoursesDTO>
                .NewConfig();
        }
    }
}
