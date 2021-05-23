using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApi.Infrastructure.Models
{
    [Table("user_token")]
    public class UserToken
    {
        /// <summary>
        /// 系統識別碼
        /// </summary>
        [Column("token_id"), Comment("系統識別碼")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenId { get; set; }

        /// <summary>
        /// 使用者識別碼
        /// </summary>
        [Column("user_id"), Comment("使用者識別碼")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者的刷新令牌
        /// </summary>
        [Column("refresh_token"), Comment("使用者的刷新令牌")]
        [StringLength(255)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 令牌生效時間
        /// </summary>
        [Column("created_at"), Comment("令牌生效時間")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 令牌生效位置
        /// </summary>
        [Column("created_by_ip"), Comment("令牌生效位置")]
        [StringLength(20)]
        public string CreatedByIp { get; set; }

        /// <summary>
        /// 令牌到期時間
        /// </summary>
        [Column("expires_at"), Comment("令牌到期時間")]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 使用哪個令牌刷新
        /// </summary>
        [Column("replaced_by_token"), Comment("使用哪個令牌刷新")]
        [StringLength(255)]
        public string ReplacedByToken { get; set; }

        /// <summary>
        /// 令牌撤銷時間
        /// </summary>
        [Column("revoked_at"), Comment("令牌撤銷時間")]
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// 令牌撤銷位置
        /// </summary>
        [Column("revoked_by_ip"), Comment("令牌撤銷位置")]
        [StringLength(20)]
        public string RevokedByIp { get; set; }
    }
}
