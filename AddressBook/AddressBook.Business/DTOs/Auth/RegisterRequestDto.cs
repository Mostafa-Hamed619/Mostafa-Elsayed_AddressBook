using System.ComponentModel.DataAnnotations;

namespace AddressBook.Business.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password cannot be less than 6 characters")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
