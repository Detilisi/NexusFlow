namespace NexusFlow.PublicApi.Models
{
    public class Account
    {
        public int Code { get; set; } = 0;
        public int PersonCode { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public float OutStandingBalance { get; set; } = 0f;

        public AccountStatus StatusCode { get; set; } = AccountStatus.Closed;
    }

    public enum AccountStatus
    {
        Open = 1,
        Closed = 0
    }
}
