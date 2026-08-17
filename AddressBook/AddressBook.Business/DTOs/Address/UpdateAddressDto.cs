using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Business.DTOs.Address
{
    public class UpdateAddressDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [Required]
        public int JobId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [RegularExpression(
            @"^01[0125][0-9]{8}$",
            ErrorMessage = "Please enter a valid Egyptian mobile number.")]
        public string MobileNumber { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(500)]
        public string AddressLine { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = null!;

        public IFormFile? Photo { get; set; }
    }
}
