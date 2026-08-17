namespace AddressBook.Business.DTOs.Address
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public int JobId { get; set; }

        public string JobTitle { get; set; } = null!;

        public int DepartmentId { get; set; }

        public string Department { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public string AddressLine { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Photo { get; set; }
    }
}
