namespace AddressBook.Business.DTOs.Address
{
    public class AddressSearchDto
    {
        public string? Search { get; set; }

        public int? JobId { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
