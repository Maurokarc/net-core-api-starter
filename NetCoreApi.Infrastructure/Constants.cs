namespace NetCoreApi.Infrastructure
{
    /// <summary>
    /// 資料庫相關常數定義
    /// </summary>
    internal static class DbConstraint
    {
        /// <summary>
        /// 資料啟用狀態定義
        /// </summary>
        public static class Enabled
        {
            /// <summary>
            /// 啟用中
            /// </summary>
            public const string ENABLED = "Y";
            /// <summary>
            /// 停用中
            /// </summary>
            public const string DISABLED = "N";
            /// <summary>
            /// 已刪除
            /// </summary>
            public const string DELETED = "D";
        }
    }
}
