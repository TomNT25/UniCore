using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.User.GetUserPermission;
using UniCore.Infrastructure.Util.Jwt;

namespace UniCore.API.AuthorizationHandler.Permission;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserService _userService;

    public PermissionAuthorizationHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimType.UserObjectId);

        if (string.IsNullOrEmpty(userId))
        {
            return; // Unauthorized
        }

        var request = new GetUserPermissionRequestDTO() { UserID = userId };

        var userPermissions = await _userService.GetUserPermissionAsync(request);

        if (userPermissions?.Permissions != null &&
            userPermissions.Permissions.Any(x => x.Name.Equals(requirement.Permission, StringComparison.OrdinalIgnoreCase)))
        {
            context.Succeed(requirement);
        }
    }
}