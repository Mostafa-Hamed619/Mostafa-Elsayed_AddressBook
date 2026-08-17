using System.ComponentModel.DataAnnotations;

namespace AddressBook.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
