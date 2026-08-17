using System.ComponentModel.DataAnnotations;

namespace AddressBook.Business.DTOs.JobTitle
{
    public class UpdateJobTitleDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
