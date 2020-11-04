using Microsoft.AspNetCore.Builder;
using RTLTechTask.Middleware;

namespace RTLTechTask.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}