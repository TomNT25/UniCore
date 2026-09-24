using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public static class GetCourseDetailsMapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CourseStudent, GetCourseDetailsResponseDTO>
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
