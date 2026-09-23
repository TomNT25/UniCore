using Microsoft.AspNetCore.Authorization;

namespace UniCore.API.AuthorizationHandler.Permission;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}