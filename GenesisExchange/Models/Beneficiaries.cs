using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenesisExchange.Models
{
    [Table("beneficiaries")]
    public class Beneficiaries
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName {  get; set; }
        [Required]
        public Bank Bank {  get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber {  get; set; }
        [Required]
        public string Sender {  get; set; }
        [Required]
        [Display(Name = "Sender CellNumber")]
        public string SenderNumber {  get; set; }
    }
}
