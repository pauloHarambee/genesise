using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenesisExchange.Models
{
    [Table("transactions")]
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public Beneficiaries Beneficiaries { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount in ZAR")]
        public decimal AmountZAR {  get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public double Rate { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string Status { get; set; }

    }
}
