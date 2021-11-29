using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class SignUpUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }
    }
}
