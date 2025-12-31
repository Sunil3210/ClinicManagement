using BLL;
using BLL.Infrastructure;
using ClinicManagement.Filters;
using ClinicManagement.Request;
using ClinicManagement.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public readonly IStaffBLL staffBLL;
        public readonly ITokenBLL tokenBLL;
        public readonly IOptions<JWTSetting> appSettings;
        public AuthController(IStaffBLL staffBLL, ITokenBLL tokenBLL, IOptions<JWTSetting> appSettings)
        {
            this.staffBLL = staffBLL;
            this.tokenBLL = tokenBLL;
            this.appSettings = appSettings;
        }

        #region Login

        /// <summary>
        /// login functionality
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        [LogActionAttribute]
        [Route("Login")]
        [HttpPost]
        public async Task<BLLResponse> Login(AuthRequest authRequest)
        {
            BLLResponse response = null;
            var staff = await staffBLL.Validate(authRequest.Email, authRequest.Password);
            var expires = appSettings.Value.TokenExpiryTimeInMinutes;
            if (staff != null)
            {
                var token = tokenBLL.GenerateAccessToken(staff, DateTime.UtcNow.AddMinutes(Convert.ToInt32(expires)));
                response = CreateSuccessResponse(token, HttpStatusCode.OK, "Login sucess");
            }
            else
            {
                response = CreateFailResponse(null, HttpStatusCode.Unauthorized, "InCorrect Email/Password");
            }

            return response;
        }

        #endregion
    }
}
