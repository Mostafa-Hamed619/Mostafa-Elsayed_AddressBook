namespace AddressBook.Business.DTOs.Address
{
    public class AddressSearchDto
    {
        public string? SearchTerm { get; set; }

        public DateTime? DateOfBirthFrom { get; set; }

        public DateTime? DateOfBirthTo { get; set; }
    }
}
