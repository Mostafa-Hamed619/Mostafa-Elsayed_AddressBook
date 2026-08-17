using System.ComponentModel.DataAnnotations;

namespace AddressBook.Business.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}