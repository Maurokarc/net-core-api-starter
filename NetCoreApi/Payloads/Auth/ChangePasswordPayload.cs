using System.ComponentModel.DataAnnotations;

namespace NetCoreApi.Payloads.Auth
{
    public class ChangePasswordPayload
    {
        /// <summary>
        /// 舊密碼
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密碼
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
