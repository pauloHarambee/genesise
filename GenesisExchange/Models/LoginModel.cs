using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GenesisExchange.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Column(TypeName = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
