using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class SignInUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
