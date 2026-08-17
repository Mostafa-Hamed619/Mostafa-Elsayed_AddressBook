using AddressBook.Business.DTOs.JobTitle;
using AddressBook.Business.Interfaces;
using AddressBook.Domain.Entities;
using AddressBook.Presentation.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Presentation.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly ApplicationDbContext _context;

        public JobTitleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobTitleDto>> GetAllAsync()
        {
            return await _context.Jobs
                .AsNoTracking()
                .Select(j => new JobTitleDto
                {
                    Id = j.Id,
                    Name = j.Name
                })
                .ToListAsync();
        }

        public async Task<JobTitleDto?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Where(j => j.Id == id)
                .Select(j => new JobTitleDto
                {
                    Id = j.Id,
                    Name = j.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<JobTitleDto> CreateAsync(
            CreateJobTitleDto dto)
        {
            var exists = await _context.Jobs
                .AnyAsync(j => j.Name == dto.Name);

            if (exists)
                throw new InvalidOperationException(
                    "Job title already exists.");

            var jobTitle = new JobTitle
            {
                Name = dto.Name
            };

            _context.Jobs.Add(jobTitle);

            await _context.SaveChangesAsync();

            return new JobTitleDto
            {
                Id = jobTitle.Id,
                Name = jobTitle.Name
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateJobTitleDto dto)
        {
            var jobTitle = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobTitle == null)
                return false;

            var exists = await _context.Jobs
                .AnyAsync(j =>
                    j.Id != id &&
                    j.Name == dto.Name);

            if (exists)
                throw new InvalidOperationException(
                    "Job title already exists.");

            jobTitle.Name = dto.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobTitle = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobTitle == null)
                return false;

            var isUsed = await _context.Addresses
                .AnyAsync(a => a.JobId == id);

            if (isUsed)
                throw new InvalidOperationException(
                    "Cannot delete job title because it is used by an address.");

            _context.Jobs.Remove(jobTitle);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}