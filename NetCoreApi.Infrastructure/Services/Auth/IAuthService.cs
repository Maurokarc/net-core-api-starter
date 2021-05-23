using NetCoreApi.Infrastructure.Models;
using NetCoreApi.Toolkit.DTO;
using System.Threading.Tasks;

namespace NetCoreApi.Infrastructure.Services.Auth
{
    /// <summary>
    /// 登入權證服務
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 帳號註冊
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="userId">操作人員</param>
        /// <returns></returns>
        Task<ResultDTO<User>> Register(string account, string password, int? userId);

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="ipAddress">使用的IP</param>
        /// <param name="ExpiresDays">刷新令牌的天數效期</param>
        /// <returns></returns>
        Task<ResultDTO<User>> Login(string account, string password, string ipAddress, int ExpiresDays);

        /// <summary>
        /// 密碼變更
        /// </summary>
        /// <param name="oldPassword">舊密碼</param>
        /// <param name="newPassword">新密碼</param>
        /// <param name="userId">操作人員識別碼</param>
        /// <returns></returns>
        Task<ResultDTO> ChangePassword(string oldPassword, string newPassword, int userId);

        /// <summary>
        /// 更新RefreshToken
        /// </summary>
        /// <param name="refreshToken">新的RefreshToken</param>
        /// <param name="ipAddress">使用的IP</param>
        /// <param name="ExpiresDays">刷新令牌的天數效期</param>
        /// <returns></returns>
        Task<ResultDTO<UserToken>> RefreshToken(string refreshToken, string ipAddress, int ExpiresDays);

        /// <summary>
        /// 使目標RefreshToken失效
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <param name="ipAddress">使用的IP</param>
        /// <param name="userId">操作人員識別碼</param>
        /// <returns></returns>
        Task<ResultDTO> RevokeToken(string refreshToken, string ipAddress, int userId);
    }
}
