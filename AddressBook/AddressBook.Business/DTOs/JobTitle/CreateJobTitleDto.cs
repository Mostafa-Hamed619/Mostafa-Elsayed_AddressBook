using System.ComponentModel.DataAnnotations;

namespace AddressBook.Business.DTOs.JobTitle
{
    public class CreateJobTitleDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
