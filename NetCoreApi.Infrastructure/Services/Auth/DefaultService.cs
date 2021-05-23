using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreApi.Infrastructure.Models;
using NetCoreApi.Toolkit.DTO;
using NetCoreApi.Toolkit.Enums;
using NetCoreApi.Toolkit.Extensions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NetCoreApi.Infrastructure.Services.Auth
{
    public sealed class DefaultService : AbstractService, IAuthService
    {
        public DefaultService(DbContextOptions dbOptions, ILoggerFactory factory) : base(dbOptions, factory) { }

        public async Task<ResultDTO<User>> Register(string account, string password, int? userId)
        {
            var existUser = await _Context.Users.Where(p => !p.DeletedBy.HasValue)
                                   .FirstOrDefaultAsync(p => p.UserEmail.Equals(account));

            if (!existUser.IsNull())
            {
                return FailResult<User>(ResultCode.PrimeKeyDuplicate);
            }

            var now = await _Context.GetSysDate();

            var user = new User
            {
                UserName = account,
                UserEmail = account,
                UserPassword = password,
                CreatedAt = now,
                CreatedBy = userId ?? default,
                UpdatedAt = now,
                UpdatedBy = userId ?? default,
                Enabled = DbConstraint.Enabled.ENABLED
            };

            _Context.Users.Add(user);

            try
            {
                await _Context.SaveChangesAsync();

                return SuccessResult(ResultCode.Success, user);
            }
            catch (Exception ex)
            {
                _Context.Rollback();

                _logger.LogError(ex, "error on IAuthService[DefaultService].Register");

                return FailResult<User>(ResultCode.CommitError);
            }
        }

        public async Task<ResultDTO<User>> Login(string account, string password, string ipAddress, int ExpiresDays)
        {
            var user = await _Context.Users.Where(p => !p.DeletedBy.HasValue)
                                               .Include(p => p.Tokens)
                                               .FirstOrDefaultAsync(p => p.UserEmail.Equals(account));

            if (user.IsNull() || !user.UserPassword.Equals(password))
            {
                return FailResult<User>(ResultCode.PasswordOrAccountError);
            }

            var now = await _Context.GetSysDate();
            user.Tokens.Add(GetRefreshToken(ipAddress, now, ExpiresDays, user.UserId));

            try
            {
                await _Context.SaveChangesAsync();

                return SuccessResult(ResultCode.Success, user);
            }
            catch (Exception ex)
            {
                _Context.Rollback();

                _logger.LogError(ex, "error on IAuthService[DefaultService].Login");

                return FailResult<User>(ResultCode.CommitError);
            }

        }

        public async Task<ResultDTO> ChangePassword(string oldPassword, string newPassword, int userId)
        {
            var user = await _Context.Users.Where(p => !p.DeletedBy.HasValue)
                                   .FirstOrDefaultAsync(p => p.UserId.Equals(userId));

            if (!user.IsNull() && user.UserPassword.Equals(oldPassword))
            {
                var now = await _Context.GetSysDate();

                user.UserPassword = newPassword;
                user.UpdatedAt = now;

                try
                {
                    await _Context.SaveChangesAsync();
                    return SuccessResult(ResultCode.Success);
                }
                catch (Exception ex)
                {
                    _Context.Rollback();

                    _logger.LogError(ex, "error on IAuthService[DefaultService].ChangePassword");

                    return FailResult(ResultCode.CommitError);
                }
            }
            else
            {
                return FailResult(ResultCode.PasswordOrAccountError);
            }
        }

        public async Task<ResultDTO<UserToken>> RefreshToken(string refreshToken, string ipAddress, int ExpiresDays)
        {
            var user = await _Context.Users.Include(p => p.Tokens)
                                               .FirstOrDefaultAsync(u => u.Tokens.Any(t => t.RefreshToken == refreshToken));

            if (user.IsNull())
            {
                return FailResult<UserToken>(ResultCode.NotFound);
            }

            var now = await _Context.GetSysDate();

            var token = user.Tokens.OrderByDescending(p => p.ExpiresAt)
                                       .FirstOrDefault(p => p.RefreshToken.Equals(refreshToken)
                                                         && p.CreatedByIp.Equals(ipAddress)
                                                         && p.ExpiresAt > now
                                                        && !p.RevokedAt.HasValue);

            if (token.IsNull())
            {
                return FailResult<UserToken>(ResultCode.NotFound);
            }

            var newToken = GetRefreshToken(ipAddress, now, ExpiresDays, user.UserId);
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = newToken.RefreshToken;

            user.Tokens.Add(newToken);

            try
            {
                await _Context.SaveChangesAsync();

                var result = SuccessResult(ResultCode.Success, newToken);
                result.DataSet = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "UserName", user.UserName}
                };

                return result;
            }
            catch (Exception ex)
            {
                _Context.Rollback();

                _logger.LogError(ex, "error on IAuthService[DefaultService].RefreshToken");

                return FailResult<UserToken>(ResultCode.CommitError);
            }

        }

        public async Task<ResultDTO> RevokeToken(string refreshToken, string ipAddress, int userId)
        {
            var user = await _Context.Users.Include(p => p.Tokens)
                                               .FirstOrDefaultAsync(u => u.Tokens.Any(t => t.RefreshToken == refreshToken));
            if (user.IsNull())
            {
                return FailResult(ResultCode.NotFound);
            }

            var now = await _Context.GetSysDate();

            var token = user.Tokens.FirstOrDefault(x => now < x.ExpiresAt && !x.RevokedAt.HasValue && x.RefreshToken == refreshToken);
            if (token.IsNull())
            {
                return FailResult(ResultCode.NotFound);
            }

            token.RevokedAt = now;
            token.RevokedByIp = ipAddress;

            try
            {
                await _Context.SaveChangesAsync();

                return SuccessResult(ResultCode.Success);
            }
            catch (Exception ex)
            {
                _Context.Rollback();

                _logger.LogError(ex, "error on IAuthService[DefaultService].RevokeToken");

                return FailResult(ResultCode.CommitError);
            }
        }

        private static UserToken GetRefreshToken(string ipAddress, DateTime createdAt, int ExpiresDays, int userId)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return new UserToken
            {
                RefreshToken = Convert.ToBase64String(randomBytes),
                ExpiresAt = createdAt.AddDays(ExpiresDays > 0 ? ExpiresDays : 7),
                CreatedAt = createdAt,
                CreatedByIp = ipAddress,
                UserId = userId,
            };
        }

    }
}
