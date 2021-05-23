namespace NetCoreApi
{
    internal static class DbConstraint
    {
        public const string PolicyKey = "CorsPolicy";
        public const string CorsAuthKey = "Authorization";
        public const string CorsAuthRefreshKey = "Refresh-Token";
        public const string DefaultNamespace = "NetCoreApi";
        public const string DefaultIssuer = "NetCoreApi";

        public class Section
        {
            public const string Author = "Author";
            public const string Log = "Logging";
            public const string Db = "Database";
            public const string Crypto = "Crypto";
        }
    }
}
