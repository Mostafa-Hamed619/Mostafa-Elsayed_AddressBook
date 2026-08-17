using System.ComponentModel.DataAnnotations;

namespace AddressBook.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [Required]
        public int JobId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Please enter a valid Egyptian mobile number.")]
        public string MobileNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(500)]
        public string AddressLine { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [StringLength(500)]
        public string? Photo { get; set; }

        public User User { get; set; } = null!;

        public JobTitle Job { get; set; } = null!;

        public Department Department { get; set; } = null!;
    }
}