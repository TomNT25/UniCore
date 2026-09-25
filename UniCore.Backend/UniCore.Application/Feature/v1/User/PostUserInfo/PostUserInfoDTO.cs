using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public class PostUserInfoDTO
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
    }
}
