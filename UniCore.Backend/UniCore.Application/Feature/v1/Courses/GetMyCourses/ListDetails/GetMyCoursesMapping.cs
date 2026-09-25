using Mapster;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Courses.GetMyCourses.InternalDTOs;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.ListDetails
{
    public class CourseStudentToDTOMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // 1. Map CourseStudent entity -> Top-level response container
            config.NewConfig<CourseStudent, GetMyCoursesDTO>()
                .Map(dest => dest.Courses, src => src.Course)
                .Map(dest => dest.StudentCourses, src => src); // Maps CourseStudent itself into StudentCourses

            // 2. Map Course entity -> CourseDto
            config.NewConfig<Course, GetCourseDTO>()
                .Map(dest => dest.Department, src => src.Department);

            // 3. Map Department entity -> DepartmentDto
            config.NewConfig<Department, GetDepartmentsDTO>()
                .Map(dest => dest.Description, src => src.Description); // Ensures Description maps correctly if present on entity

            // 4. Map CourseStudent entity -> StudentCourseDto
            config.NewConfig<CourseStudent, GetStudentCoursesDTO>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.EndDate, src => src.EndDate)
                .Map(dest => dest.Weight, src => src.Weight)
                .Map(dest => dest.FinalScore, src => src.FinalScore)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Schedules, src => src.Schedules);

            // 5. Map Schedule entity -> ScheduleDto
            config.NewConfig<Schedule, GetCourseDetailsScheduleDTO>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.StudentCourseId, src => src.StudentCourseId)
                .Map(dest => dest.TimeSlot, src => src.TimeSlot)
                .Map(dest => dest.DayOccur, src => src.DayOccur)
                .Map(dest => dest.IsActive, src => src.IsActive);
        }
    }
}
