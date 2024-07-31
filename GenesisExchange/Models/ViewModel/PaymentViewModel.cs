using System.ComponentModel.DataAnnotations;

namespace GenesisExchange.Models.ViewModel
{
    public class PaymentViewModel
    {
        [Required]
        public string AccountNumber {  get; set; }
        [Required]
        public double Rate {  get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
