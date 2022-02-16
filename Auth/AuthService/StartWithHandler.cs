using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.AuthService
{
    public class StartWithHandler : AuthorizationHandler<StartWithRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StartWithRequirement requirement)
        {
            var currentUserName = context.User.FindFirst(u => u.Type == ClaimTypes.Name).Value;
            var currentUsernameStartWithLetter = currentUserName[0].ToString();


            if (currentUsernameStartWithLetter == requirement.StartWithLetter)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
