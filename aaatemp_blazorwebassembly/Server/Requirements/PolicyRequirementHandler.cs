using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aaatemp_blazorwebassembly.Server.Requirements
{
    public class PolicyRequirementHandler : AuthorizationHandler<PolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PolicyRequirement requirement)
        {
            //TODO: Add check if user in in Authorized Users table

            //var authFilterContext = context.Resource as AuthorizationFilterContext;
            //if (authFilterContext == null)
            //{
            //    return Task.CompletedTask;
            //}

            string roleName = requirement.PolicyName.Replace("Policy", "");
            if (!context.User.IsInRole(roleName))
            {
                context.Fail();
            }

            if (!context.User.HasClaim(
                c => c.Type == $"{roleName}_StartDate"
                && DateTime.TryParse(c.Value, out DateTime dtRoleStart)
                && DateTime.Now >= dtRoleStart)
            )
            {
                context.Fail();
            }

            if (!context.User.HasClaim(
                c => c.Type == $"{roleName}_EndDate"
                && DateTime.TryParse(c.Value, out DateTime dtRoleEnd)
                && DateTime.Now <= dtRoleEnd)
)
            {
                context.Fail();
            }

            if (!context.HasFailed)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
