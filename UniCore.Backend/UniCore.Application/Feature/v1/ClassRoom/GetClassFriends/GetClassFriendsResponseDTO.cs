using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsResponseDTO
    {
        public IEnumerable<GetClassFriendsDTO> ClassmatesList { get; set; } = [];
    }
}
