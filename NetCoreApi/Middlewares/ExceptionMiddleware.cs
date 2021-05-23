using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetCoreApi.Extensions;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NetCoreApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory factory)
        {
            _logger = factory.CreateDomainLogger();
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
                _logger.LogCritical(ex, "Catch error in exception middleware");
                await HandleExceptionAsync(httpContext);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error from the exception middleware."
            }.ToString());
        }
    }
}
