using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ClinicManagement.Filters
{
    public class LogActionAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var env = context.HttpContext.RequestServices
                        .GetRequiredService<IWebHostEnvironment>();

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var log = $"{time} | {method} | {controller}/{action} | {path}";

            WriteLog(env.WebRootPath, log);
        }

        private void WriteLog(string webRootPath, string message)
        {
            var logDir = Path.Combine(webRootPath, "logs");

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            var filePath = Path.Combine(logDir, $"log_{DateTime.Now:yyyyMMdd}.txt");

            using var writer = new StreamWriter(filePath, true, Encoding.UTF8);
            writer.WriteLine(message);
        }
    }
}
