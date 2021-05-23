using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApi.Infrastructure.Models
{
    /// <summary>
    /// 會員資料表
    /// </summary>
    [Table("user"), Comment("會員資料表")]
    public class User
    {
        /// <summary>
        /// 系統識別碼
        /// </summary>
        [Required]
        [Column("user_id"), Comment("系統識別碼")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        /// <summary>
        /// 員工名稱
        /// </summary>
        [Required, StringLength(50)]
        [Column("user_name"), Comment("員工名稱")]
        public string UserName { get; set; }

        /// <summary>
        /// 員工密碼
        /// </summary>
        [Required, StringLength(200)]
        [Column("user_password"), Comment("員工密碼")]
        public string UserPassword { get; set; }

        /// <summary>
        /// 使用者信箱
        /// </summary>
        [StringLength(255)]
        [Column("user_email"), Comment("使用者信箱")]
        public string UserEmail { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        [StringLength(20)]
        [Column("user_phone"), Comment("連絡電話")]
        public string UserPhone { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(255)]
        [Column("user_remark"), Comment("備註")]
        public string UserRemark { get; set; }

        /// <summary>
        /// 啟用狀態(Y/N/D)
        /// </summary>
        [Required, StringLength(1)]
        [Column("enabled"), Comment("啟用狀態(Y/N/D)")]
        public string Enabled { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at"), Comment("建立時間")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Column("created_by"), Comment("建立人員")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Required]
        [Column("updated_at"), Comment("更新時間")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        [Required]
        [Column("updated_by"), Comment("更新人員")]
        public int UpdatedBy { get; set; }

        /// <summary>
        /// 刪除時間
        /// </summary>
        [Column("deleted_at"), Comment("刪除時間")]
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 刪除人員
        /// </summary>
        [Column("deleted_by"), Comment("刪除人員")]
        public int? DeletedBy { get; set; }

        public System.Collections.Generic.List<UserToken> Tokens { get; set; }
    }
}
