using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.AuthService
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            var currrentUser = context.User.FindFirst(i => i.Type == ClaimTypes.DateOfBirth).Value;
            var userAgeTimeSpan = DateTime.Now - Convert.ToDateTime(currrentUser);
            var userAgeDays = userAgeTimeSpan.TotalDays;
            var userAge = userAgeDays / 365;
            if (userAge >= requirement.BirthDate)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}