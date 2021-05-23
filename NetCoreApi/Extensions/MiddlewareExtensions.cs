using Microsoft.AspNetCore.Builder;
using NetCoreApi.Middlewares;

namespace NetCoreApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void SetMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
