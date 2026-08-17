using AddressBook.Business.DTOs.JobTitle;

namespace AddressBook.Business.Interfaces
{
    public interface IJobTitleService
    {
        Task<IEnumerable<JobTitleDto>> GetAllAsync();

        Task<JobTitleDto?> GetByIdAsync(int id);

        Task<JobTitleDto> CreateAsync(CreateJobTitleDto dto);

        Task<bool> UpdateAsync(int id, UpdateJobTitleDto dto);

        Task<bool> DeleteAsync(int id);
    }
}