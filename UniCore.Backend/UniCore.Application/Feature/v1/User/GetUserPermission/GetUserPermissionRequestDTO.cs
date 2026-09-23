using System;
using System.Collections.Generic;
using System.Text;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.User.GetUserPermission
{
    public class GetUserPermissionRequestDTO : IRequest<GetUserPermissionResponseDTO>
    {
        public string UserID { get; set; }
    }
}
