using System.ComponentModel.DataAnnotations;

namespace NexusFlow.PublicApi.Models
{
    public class Transaction
    {
        public int Code { get; set; } = 0;
        public int AccountCode { get; set; } = 0;
        public decimal Amount { get; set; } = 00.00m;
        public string Description { get; set; } = string.Empty;
        
        public DateTime CaptureDate { get; set; } = DateTime.Now;
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
