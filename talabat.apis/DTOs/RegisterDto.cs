using System.ComponentModel.DataAnnotations;

namespace talabat.apis.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; } 
    }
}
