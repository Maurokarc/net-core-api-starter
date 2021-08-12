using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreApi.Authentications;
using NetCoreApi.DTO;
using NetCoreApi.Infrastructure.Services.Auth;
using NetCoreApi.Payloads.Auth;
using NetCoreApi.Toolkit.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApi.Controllers.v2
{
    [Authorize]
    [ApiVersion("2.0")]
    public class AuthController : ApiController
    {
        private readonly IAuthor _author;
        private readonly IAuthService _service;

        public AuthController(ILoggerFactory factory, IAuthor author, IAuthService service) : base(factory)
        {
            _author = author;
            _service = service;
        }

#if DEBUG

        /// <summary>
        /// 登入驗證(Swagger專用 , 僅在Debug下顯示) 
        /// </summary>
        /// <param name="dto">登入相關資料</param>
        /// <returns></returns>
        /// <response code="200">回傳JWT</response>
        /// <response code="400">帳號或密碼輸入不正確 , 驗證失敗</response>
        [HttpPost, Route("swagger-login")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SwaggerLogin([FromForm] LoginPayload dto)
        {
            var payload = await _service.Login(dto.Account, dto.Password, GetIpAddress(), _author.RefreshExpireDays);

            if (!payload.IsSucceed)
            {
                return BadRequest(payload);
            }

            string token = _author.Generate(payload.Meta.UserId.ToString(), payload.Meta.UserName);
            string refreshToken = payload.Meta.Tokens.OrderByDescending(p => p.ExpiresAt).FirstOrDefault().RefreshToken;
            Response.Headers.Add(DbConstraint.CorsAuthKey, token);
            Response.Headers.Add(DbConstraint.CorsAuthRefreshKey, refreshToken);

            return NoContent();
        }

#endif

        /// <summary>
        /// 取得Jwt的資料
        /// </summary>
        /// <returns></returns>
        /// <response code="200">回傳登入資訊</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdentityDTO))]
        public IActionResult Get()
        {
            var identity = _author.GetIdentity(User);

            string token = _author.Generate(identity.UserId, identity.UserName);
            Response.Headers.Add(DbConstraint.CorsAuthKey, token);

            return Ok(identity);
        }

    }
}
