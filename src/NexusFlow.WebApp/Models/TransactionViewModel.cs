using System.ComponentModel.DataAnnotations;

namespace NexusFlow.WebApp.Models
{
    public class TransactionViewModel
    {
        public int Code { get; set; } = 0;
        public int AccountCode { get; set; } = 0;
        public decimal Amount { get; set; } = 00.00m;
        public string Description { get; set; } = string.Empty;
        public TransactionType Type { get; set; } = TransactionType.Debit;

        public DateTime CaptureDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [CustomDateValidation(ErrorMessage = "Transaction date cannot be in the future.")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
    public enum TransactionType
    {
        Credit = 1,
        Debit = 0
    }

    public class CustomDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (date > DateTime.Now)
            {
                return new ValidationResult("The date cannot be in the future.");
            }

            return ValidationResult.Success;
        }
    }

}
