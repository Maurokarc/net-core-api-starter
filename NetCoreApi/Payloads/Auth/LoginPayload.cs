using System.ComponentModel.DataAnnotations;

namespace NetCoreApi.Payloads.Auth
{
    public class LoginPayload
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 登入密碼
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
