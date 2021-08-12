using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;

namespace NetCoreApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiController, ApiVersion("1.0")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Application.Soap)]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ILogger _logger;

        public ApiController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(this.GetType().UnderlyingSystemType.Name);
        }

        protected void SetCookie(string key, string content, int expiresDays = 7)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(expiresDays > 0 ? expiresDays : 7)
            };
            Response.Cookies.Append(key, content, cookieOptions);
        }

        protected string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
