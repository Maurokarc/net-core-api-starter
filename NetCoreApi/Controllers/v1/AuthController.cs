using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreApi.Authentications;
using NetCoreApi.DTO;
using NetCoreApi.Infrastructure.Services.Auth;
using NetCoreApi.Payloads.Auth;
using NetCoreApi.Toolkit;
using NetCoreApi.Toolkit.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApi.Controllers.v1
{
    [Authorize]
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


        /// <summary>
        /// 密碼變更(Swagger專用 , 僅在Debug下顯示) 
        /// </summary>
        /// <param name="dto">密碼變更相關資料</param>
        /// <returns></returns>
        /// <response code="204">變更成功</response>
        /// <response code="400">變更失敗</response>
        [HttpPost, Route("swagger-change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> SwaggerChangePassword([FromForm] ChangePasswordPayload dto)
        {
            var identity = _author.GetIdentity(User);
            var result = await _service.ChangePassword(dto.OldPassword, dto.NewPassword, Convert.ToInt32(identity.UserId));
            if (result.IsSucceed)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result);
            }
        }

#endif

        /// <summary>
        /// 帳號註冊
        /// </summary>
        /// <param name="dto">登入相關資料</param>
        /// <returns></returns>
        /// <response code="200">回傳建立成功的資料</response>
        /// <response code="400">帳號或密碼輸入不正確 , 驗證失敗</response>
        [AllowAnonymous]
        [HttpPost, Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Infrastructure.Models.User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] LoginPayload dto)
        {
            var payload = await _service.Register(dto.Account, dto.Password, null);

            if (!payload.IsSucceed)
            {
                return BadRequest(payload);
            }
            else
            {
                return Ok(payload.Meta);
            }
        }


        /// <summary>
        /// 登入驗證 
        /// </summary>
        /// <param name="crypto">加解密服務</param>
        /// <param name="dto">登入相關資料</param>
        /// <returns></returns>
        /// <response code="200">回傳JWT</response>
        /// <response code="400">帳號或密碼輸入不正確 , 驗證失敗</response>
        [AllowAnonymous]
        [HttpPost, Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Login([FromServices] CryptoService crypto, [FromForm] LoginPayload dto)
        {
            var payload = await _service.Login(dto.Account, crypto.Decrypt(dto.Password), GetIpAddress(), _author.RefreshExpireDays);

            if (!payload.IsSucceed)
            {
                return BadRequest(payload);
            }

            string token = _author.Generate(payload.Meta.UserId.ToString(), payload.Meta.UserName);
            string refreshToken = payload.Meta.Tokens.OrderByDescending(p => p.ExpiresAt).FirstOrDefault().RefreshToken;
            Response.Headers.Add(DbConstraint.CorsAuthKey, token);
            Response.Headers.Add(DbConstraint.CorsAuthRefreshKey, refreshToken);
            return Ok();
        }

        /// <summary>
        /// 密碼變更
        /// </summary>
        /// <param name="crypto">加解密服務</param>
        /// <param name="dto">密碼變更相關資料</param>
        /// <returns></returns>
        /// <response code="204">變更成功</response>
        /// <response code="400">變更失敗</response>
        [HttpPost, Route("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChangePassword([FromServices] CryptoService crypto, [FromForm] ChangePasswordPayload dto)
        {
            var identity = _author.GetIdentity(User);
            var result = await _service.ChangePassword(crypto.Decrypt(dto.OldPassword), crypto.Decrypt(dto.NewPassword), Convert.ToInt32(identity.UserId));
            if (result.IsSucceed)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result);
            }
        }

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

        /// <summary>
        /// 令牌刷新
        /// </summary>
        /// <returns></returns>
        /// <response code="201">刷新成功</response>
        /// <response code="400">刷新失敗</response>
        [AllowAnonymous]
        [HttpPost, Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> RefreshToken([Required, FromHeader] string refreshToken)
        {
            var result = await _service.RefreshToken(refreshToken, GetIpAddress(), _author.RefreshExpireDays);

            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            string token = _author.Generate(result.Meta.UserId.ToString(), result.DataSet[nameof(IdentityDTO.UserName)].ToString());
            string newRefreshToken = result.Meta.RefreshToken;
            Response.Headers.Add(DbConstraint.CorsAuthKey, token);
            Response.Headers.Add(DbConstraint.CorsAuthRefreshKey, newRefreshToken);

            return NoContent();
        }

        /// <summary>
        /// 令牌註銷
        /// </summary>
        /// <returns></returns>
        /// <response code="201">註銷成功</response>
        /// <response code="400">註銷失敗</response>
        [HttpPost, Route("revoke-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> RevokeToken([Required, FromHeader] string refreshToken)
        {
            var identity = _author.GetIdentity(User);
            var result = await _service.RevokeToken(refreshToken, GetIpAddress(), Convert.ToInt32(identity.UserId));

            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }
            else
            {
                return NoContent();
            }
        }

    }
}
