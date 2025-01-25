namespace NexusFlow.PublicApi.Models
{
    public class Account
    {
        public int Code { get; set; } = 0;
        public int PersonCode { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal OutstandingBalance { get; set; } = 00.00m;
        public int StatusCode { get; set; } = (int)AccountStatus.Closed;
    }

    public enum AccountStatus
    {
        Open = 1,
        Closed = 0
    }
}
