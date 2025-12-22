using System.Security.Claims;

namespace ClinicManagement.Extension
{
    public interface IUserClaimService
    {
        string GetUserId();
        string GetRole();
    }
    public class UserClaimService : IUserClaimService
    {
        public readonly IHttpContextAccessor httpContextAccessor;
        public UserClaimService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            //var claims = httpContextAccessor.HttpContext.User.Claims;
            return httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetRole()
        {
            return httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Role);
        }

        public string GetName()
        {
            return httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
        }
    }
}
