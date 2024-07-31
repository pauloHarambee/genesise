using System.ComponentModel.DataAnnotations;

namespace GenesisExchange.Models.ViewModel
{
    public class BeneficiaryViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Bank")]
        public int BankId { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Required]
        [Display(Name = "Sender Name")]
        public string Sender { get; set; }
        [Required]
        [Display(Name = "Sender CellNumber")]
        public string SenderNumber { get; set; }
    }
}
