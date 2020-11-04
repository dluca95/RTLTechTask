using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;

namespace RTLTechTask.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
 
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
 
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
 
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                ArgumentNullException e => (int) HttpStatusCode.BadRequest,
                ArgumentException e => (int) HttpStatusCode.BadRequest,
                _ => (int) HttpStatusCode.InternalServerError
            };

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Stacktrace = exception.StackTrace
            }.ToString());
        }
    }
}