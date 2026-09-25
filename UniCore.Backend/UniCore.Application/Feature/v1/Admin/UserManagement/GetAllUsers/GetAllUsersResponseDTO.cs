using UniCore.Application.DTO;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersResponseDTO{
        public IEnumerable<GetAllUsersItemDTO> Items { get; set; } = [];
        public GetAllUserMetadataDTO Metadata { get; set; } = new();
    }

    public class GetAllUsersItemDTO {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } 
        public string StudentCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime EmailVerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool? IsProfileVerified { get; set; }
    }

    // LÁT NHỚ TỰ KIẾM LẠI CÁI TYPE GỐC, T TỰ DEFINE DO KO KIẾM ĐƯỢC NITO 
    public class GetAllUserMetadataDTO {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
