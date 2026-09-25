using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public class PostUserInfoDTO
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? gender { get; set; }
        public string? birthDate { get; set; }
        public string? address { get; set; }
        public string? bio { get; set; }
    }
}
