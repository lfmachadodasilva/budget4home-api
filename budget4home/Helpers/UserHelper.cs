using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace budget4home.Helpers
{
    public static class UserHelper
    {
        /// <inheritdoc>
        public static string GetUserId(HttpContext httpContext)
        {
            var identity = httpContext?.User?.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return null;
            }

            IEnumerable<Claim> claims = identity.Claims;
            var claim = claims.FirstOrDefault(x => x.Type.Equals("user_id"));
            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }
    }
}