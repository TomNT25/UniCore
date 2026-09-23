using System;

namespace UniCore.Application.DTO
{
    public class CursorPaginationRequest : PaginationRequest
    {
        public string? Cursor { get; set; }
    }
}
