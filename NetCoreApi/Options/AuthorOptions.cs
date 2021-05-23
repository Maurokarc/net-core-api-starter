namespace NetCoreApi.Options
{
    public class AuthorOptions
    {
        /// <summary>
        /// 發行者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 令牌的效期(分鐘)
        /// </summary>
        public int? ExpireMinutes { get; set; }
        /// <summary>
        /// 刷新令牌的效期(天)
        /// </summary>
        public int? RefreshExpireDays { get; set; }
    }
}
