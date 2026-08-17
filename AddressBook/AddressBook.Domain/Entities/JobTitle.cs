using System.ComponentModel.DataAnnotations;

namespace AddressBook.Domain.Entities
{
    public class JobTitle
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
