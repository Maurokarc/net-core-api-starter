using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreApi.Infrastructure.Contexts;
using NetCoreApi.Toolkit.DTO;
using NetCoreApi.Toolkit.Enums;
using System;

namespace NetCoreApi.Infrastructure.Services
{
    public abstract class AbstractService : IDisposable
    {
        protected readonly ILogger _logger;
        internal readonly ServerContext _Context;

        public AbstractService(DbContextOptions dbOptions, ILoggerFactory factory)
        {
            _Context = new ServerContext(dbOptions);
            _logger = factory.CreateLogger<AbstractService>();
        }

        #region Create Result

        protected static ResultDTO SuccessResult(ResultCode code)
        {
            return SuccessResult(code, string.Empty, default);
        }

        protected static ResultDTO SuccessResult(ResultCode code, string message)
        {
            return SuccessResult(code, message, default);
        }

        protected static ResultDTO SuccessResult(ResultCode code, object meta)
        {
            return SuccessResult(code, string.Empty, meta);
        }

        protected static ResultDTO SuccessResult(ResultCode code, string message, object meta)
        {
            return new ResultDTO { IsSucceed = true, ResultCode = code, Message = message, Meta = meta };
        }

        protected static ResultDTO FailResult(ResultCode code)
        {
            return FailResult(code, string.Empty, default);
        }

        protected static ResultDTO FailResult(ResultCode code, string message)
        {
            return FailResult(code, message, default);
        }

        protected static ResultDTO FailResult(ResultCode code, string message, object meta)
        {
            return new ResultDTO { IsSucceed = false, ResultCode = code, Message = message, Meta = meta };
        }

        protected static ResultDTO<T> SuccessResult<T>(ResultCode code)
        {
            return GenerateResult(true, code, string.Empty, default(T));
        }

        protected static ResultDTO<T> SuccessResult<T>(ResultCode code, string message)
        {
            return GenerateResult(true, code, message, default(T));
        }

        protected static ResultDTO<T> SuccessResult<T>(ResultCode code, T meta)
        {
            return GenerateResult(true, code, string.Empty, meta);
        }

        protected static ResultDTO<T> SuccessResult<T>(ResultCode code, string message, T meta)
        {
            return GenerateResult(true, code, message, meta);
        }

        protected static ResultDTO<T> FailResult<T>(ResultCode code)
        {
            return GenerateResult(false, code, string.Empty, default(T));
        }

        protected static ResultDTO<T> FailResult<T>(ResultCode code, string message)
        {
            return GenerateResult(false, code, message, default(T));
        }

        protected static ResultDTO<T> FailResult<T>(ResultCode code, string message, T meta)
        {
            return GenerateResult(false, code, message, meta);
        }

        protected static ResultDTO<T> GenerateResult<T>(bool success, ResultCode code, string message, T meta = default)
        {
            return new ResultDTO<T> { IsSucceed = success, ResultCode = code, Message = message, Meta = meta };
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
