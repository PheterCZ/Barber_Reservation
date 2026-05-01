using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;

namespace backend.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
    }
}

