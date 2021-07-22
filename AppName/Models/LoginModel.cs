using System.ComponentModel.DataAnnotations;

namespace PerformPro.Models
{
    public class LoginModel
    {
        //Email used to login
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        [MaxLength(320)]
        public string Email { get; set; }

        //User password
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
