using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ClinicManagement.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var path = context.HttpContext.Request.Path;
            var method = context.HttpContext.Request.Method;
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var logMessage = $@"
            [{time}]
            Controller : {controller}
            Action     : {action}
            Method     : {method}
            Path       : {path}
            Message    : {exception.Message}
            StackTrace : {exception.StackTrace}
            --------------------------------------------------";

            WriteLog(logMessage);

            context.Result = new ObjectResult(new
            {
                success = false,
                message = "An internal server error occurred."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void WriteLog(string message)
        {
            var logDir = Path.Combine(_env.WebRootPath, "ErrorLog");

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            var filePath = Path.Combine(logDir, $"exceptions_{DateTime.Now:yyyyMMdd}.txt");

            using var writer = new StreamWriter(filePath, true, Encoding.UTF8);
            writer.WriteLine(message);
        }
    }
}
