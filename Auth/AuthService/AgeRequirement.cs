using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.AuthService
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public double BirthDate { get; set; }
        public AgeRequirement(double birth)
        {
            BirthDate = birth;
        }
    }
}