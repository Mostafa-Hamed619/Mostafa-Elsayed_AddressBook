using AddressBook.Business.DTOs.Department;

namespace AddressBook.Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();

        Task<DepartmentDto?> GetByIdAsync(int id);

        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

        Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}