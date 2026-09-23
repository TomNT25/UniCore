using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using UniCore.API.AuthorizationHandler.Permission;
// using UniCore.API.AuthorizationHandler.Role;

namespace UniCore.API.AuthorizationHandler
{
    public class DynamicAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                // .AddRequirements(new RoleRequirement(policyName))
                .Build();
        }
    }
}