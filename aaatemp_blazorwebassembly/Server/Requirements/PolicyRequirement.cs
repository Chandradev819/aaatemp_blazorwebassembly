using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aaatemp_blazorwebassembly.Server.Requirements
{
    public class PolicyRequirement : IAuthorizationRequirement
    {
        public PolicyRequirement(string policyName)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; }
    }
}
