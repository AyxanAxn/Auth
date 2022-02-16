using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.AuthService
{
    public class StartWithRequirement : IAuthorizationRequirement
    {
        public string StartWithLetter { get; set; }
        public StartWithRequirement(string startWithLetter)
        {
            StartWithLetter = startWithLetter;
        }
    }
}
