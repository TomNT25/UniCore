using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class AnnouncementTargetCursorPaginationRequest : CursorPaginationRequest
    {
        public string? Search
        {
            get => SearchTerm;
            set => SearchTerm = value;
        }

        public int? Limit
        {
            get => PageSize;
            set
            {
                if (value.HasValue)
                {
                    PageSize = value.Value;
                }
            }
        }
    }

    public class SearchCourseTargetsRequestDTO : AnnouncementTargetCursorPaginationRequest
    {
    }

    public class SearchClassTargetsRequestDTO : AnnouncementTargetCursorPaginationRequest
    {
    }

    public class SearchDepartmentTargetsRequestDTO : AnnouncementTargetCursorPaginationRequest
    {
    }

    public class SearchStudentTargetsRequestDTO : AnnouncementTargetCursorPaginationRequest
    {
    }

    public class SearchCourseTargetsResponseDTO : CursorPaginationResponse<CourseTargetItemDto>
    {
        public SearchCourseTargetsResponseDTO() : base() { }
        public SearchCourseTargetsResponseDTO(IEnumerable<CourseTargetItemDto> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata) { }
    }

    public class SearchClassTargetsResponseDTO : CursorPaginationResponse<ClassTargetItemDto>
    {
        public SearchClassTargetsResponseDTO() : base() { }
        public SearchClassTargetsResponseDTO(IEnumerable<ClassTargetItemDto> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata) { }
    }

    public class SearchDepartmentTargetsResponseDTO : CursorPaginationResponse<DepartmentTargetItemDto>
    {
        public SearchDepartmentTargetsResponseDTO() : base() { }
        public SearchDepartmentTargetsResponseDTO(IEnumerable<DepartmentTargetItemDto> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata) { }
    }

    public class SearchStudentTargetsResponseDTO : CursorPaginationResponse<StudentTargetItemDto>
    {
        public SearchStudentTargetsResponseDTO() : base() { }
        public SearchStudentTargetsResponseDTO(IEnumerable<StudentTargetItemDto> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata) { }
    }

    public class AnnouncementTargetSearchQuery : AnnouncementTargetCursorPaginationRequest
    {
    }

    public class AnnouncementTargetSearchResponseDto<T> : CursorPaginationResponse<T>
    {
        public AnnouncementTargetSearchResponseDto() : base() { }
        public AnnouncementTargetSearchResponseDto(IEnumerable<T> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata) { }
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
