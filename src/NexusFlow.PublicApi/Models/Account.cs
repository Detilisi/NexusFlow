namespace NexusFlow.PublicApi.Models
{
    public class Account
    {
        public int Code { get; set; } = 0;
        public int PersonCode { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal OutStandingBalance { get; set; } = 00.00m;

        public AccountStatus Status { get; set; } = AccountStatus.Open;
    }

    public enum AccountStatus
    {
        Open = 1,
        Closed = 0
    }
}
