using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class AnnouncementTargetSearchQuery
    {
        public string? Search { get; set; }
        public int? Limit { get; set; }
    }

    public class AnnouncementTargetSearchMetaDto
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public string? Search { get; set; }
        public string Domain { get; set; } = string.Empty;
    }

    public class AnnouncementTargetSearchResponseDto<T>
    {
        public List<T> Data { get; set; } = new();
        public AnnouncementTargetSearchMetaDto Meta { get; set; } = new();
    }

    public class CourseTargetItemDto
    {
        [JsonPropertyName("course_id")]
        public string CourseId { get; set; } = string.Empty;

        [JsonPropertyName("course_name")]
        public string CourseName { get; set; } = string.Empty;

        [JsonPropertyName("course_code")]
        public string? CourseCode { get; set; }
    }

    public class ClassTargetItemDto
    {
        [JsonPropertyName("class_id")]
        public string ClassId { get; set; } = string.Empty;

        [JsonPropertyName("class_name")]
        public string ClassName { get; set; } = string.Empty;

        [JsonPropertyName("class_code")]
        public string? ClassCode { get; set; }
    }

    public class DepartmentTargetItemDto
    {
        [JsonPropertyName("department_id")]
        public string DepartmentId { get; set; } = string.Empty;

        [JsonPropertyName("department_name")]
        public string DepartmentName { get; set; } = string.Empty;

        [JsonPropertyName("department_code")]
        public string? DepartmentCode { get; set; }
    }

    public class StudentTargetItemDto
    {
        [JsonPropertyName("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [JsonPropertyName("student_name")]
        public string StudentName { get; set; } = string.Empty;

        [JsonPropertyName("student_code")]
        public string? StudentCode { get; set; }
    }
}
