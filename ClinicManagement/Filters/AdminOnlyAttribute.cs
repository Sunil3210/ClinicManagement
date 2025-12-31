using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ClinicManagement.Filters
{
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // 1️⃣ Check if user is authenticated
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult(); // 401
                return;
            }

            var isAdmin = user.Claims.Any(c =>
                c.Type == ClaimTypes.Role && c.Value == "Admin");

            if (!isAdmin)
            {
                context.Result = new ForbidResult(); // 403
                return;
            }
        }

    }
}
