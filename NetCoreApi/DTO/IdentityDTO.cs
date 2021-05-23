using System;

namespace NetCoreApi.DTO
{
    public class IdentityDTO
    {
        /// <summary>
        /// 登入人員識別碼
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 登入人員名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 令牌發行時間
        /// </summary>
        public DateTime? AuthorizeTime { get; set; }
        /// <summary>
        /// 令牌失效期限
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
