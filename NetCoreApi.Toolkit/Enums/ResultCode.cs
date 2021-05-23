namespace NetCoreApi.Toolkit.Enums
{
    public enum ResultCode
    {
        Empty,
        Success = 2000,
        NotFound = 3000,
        PasswordOrAccountError = 9001,
        PrimeKeyDuplicate = 9002,
        CommitError = 9003,
        ConstantError = 9004
    }
}
