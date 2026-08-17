using AddressBook.Business.DTOs.Address;

namespace AddressBook.Business.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAllAsync();

        Task<AddressDto?> GetByIdAsync(int id);

        Task<AddressDto> CreateAsync(CreateAddressDto dto);

        Task<bool> UpdateAsync(int id, UpdateAddressDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<AddressDto>> SearchAsync(AddressSearchDto dto);

        Task<byte[]> ExportToExcelAsync();
    }
}
