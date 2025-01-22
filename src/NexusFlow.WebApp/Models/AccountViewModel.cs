namespace NexusFlow.WebApp.Models
{
    public class AccountViewModel
    {
        public int Code { get; set; } = 0;
        public int PersonCode { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal OutStandingBalance { get; set; } = 00.00m;
        public List<TransactionViewModel> Transactions { get; set; } = [];
    }
}
