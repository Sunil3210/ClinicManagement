using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class JWTSetting
    {
        public string AccessTokenKey { get; set; }
        public string TokenExpiryTimeInMinutes { get; set; }
        public string CookieExpriryTimeMinutes { get; set; }
        public string Issuer { get; set; }
    }
}
