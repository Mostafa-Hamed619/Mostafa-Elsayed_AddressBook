using AddressBook.Business.DTOs.Address;
using AddressBook.Business.Interfaces;
using AddressBook.Domain.Entities;
using AddressBook.Presentation.Data;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
namespace AddressBook.Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _environment;

        public AddressService(ApplicationDbContext context, ICurrentUserService currentUserService, IWebHostEnvironment environment)
        {
            _context = context;
            _currentUserService = currentUserService;
            _environment = environment;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            var userId = _currentUserService.GetUserId();

            var addresses = await _context.Addresses.AsNoTracking()
                .Where(a => a.UserId == userId)
                .Include(a => a.Job)
                .Include(a => a.Department)
                .ToListAsync();

            return addresses.Select(MapToDto);
        }

        public async Task<AddressDto?> GetByIdAsync(int id)
        {
            var userId = _currentUserService.GetUserId();

            var address = await _context.Addresses
                .AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Department)
                .FirstOrDefaultAsync(
                    a => a.Id == id && a.UserId == userId);

            return address == null ? null : MapToDto(address);
        }

        public async Task<AddressDto> CreateAsync(CreateAddressDto dto)
        {
            var userId = _currentUserService.GetUserId();
            var userEmail = _currentUserService.GetUserEmail();

            var jobExists = await _context.Jobs.AnyAsync(j => j.Id == dto.JobId);

            if (!jobExists)
                throw new KeyNotFoundException("Job title not found.");

            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == dto.DepartmentId);

            if (!departmentExists)
                throw new KeyNotFoundException("Department not found.");

            string? photoPath = null;

            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "addresses");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(dto.Photo.FileName);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);

                await dto.Photo.CopyToAsync(stream);

                photoPath = $"/uploads/addresses/{fileName}";
            }

            var address = new Address
            {
                UserId = userId,
                Email = userEmail,
                FullName = dto.FullName,
                JobId = dto.JobId,
                DepartmentId = dto.DepartmentId,
                MobileNumber = dto.MobileNumber,
                DateOfBirth = dto.DateOfBirth,
                AddressLine = dto.AddressLine,
                Photo = photoPath
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return (await GetByIdAsync(address.Id))!;
        }


        public async Task<bool> UpdateAsync(int id, UpdateAddressDto dto)
        {
            var userId = _currentUserService.GetUserId();

            var address = await _context.Addresses
                .FirstOrDefaultAsync(a =>
                    a.Id == id &&
                    a.UserId == userId);

            if (address == null)
                return false;

            var jobExists = await _context.Jobs
                .AnyAsync(j => j.Id == dto.JobId);

            if (!jobExists)
                throw new KeyNotFoundException(
                    "Job title not found.");

            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == dto.DepartmentId);

            if (!departmentExists)
                throw new KeyNotFoundException(
                    "Department not found.");

            address.FullName = dto.FullName;
            address.JobId = dto.JobId;
            address.DepartmentId = dto.DepartmentId;
            address.MobileNumber = dto.MobileNumber;
            address.DateOfBirth = dto.DateOfBirth;
            address.AddressLine = dto.AddressLine;

            // update photo only if a new photo was uploaded
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "addresses");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(dto.Photo.FileName);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(
                    uploadsFolder,
                    fileName);

                using var stream = new FileStream(
                    filePath,
                    FileMode.Create);

                await dto.Photo.CopyToAsync(stream);

                address.Photo = $"/uploads/addresses/{fileName}";
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userId = _currentUserService.GetUserId();

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return false;

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AddressDto>> SearchAsync(AddressSearchDto dto)
        {
            var query = _context.Addresses
                .Include(a => a.Job)
                .Include(a => a.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.SearchTerm))
            {
                var term = dto.SearchTerm.Trim();

                query = query.Where(a =>
                    a.FullName.Contains(term) ||
                    a.Email.Contains(term) ||
                    a.MobileNumber.Contains(term) ||
                    a.AddressLine.Contains(term) ||
                    a.Job.Name.Contains(term) ||
                    a.Department.Name.Contains(term)
                );
            }

            if (dto.DateOfBirthFrom.HasValue)
            {
                query = query.Where(a =>
                    a.DateOfBirth >= dto.DateOfBirthFrom.Value);
            }

            if (dto.DateOfBirthTo.HasValue)
            {
                query = query.Where(a =>
                    a.DateOfBirth <= dto.DateOfBirthTo.Value);
            }

            return await query
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    JobId = a.JobId,
                    JobTitle = a.Job.Name,
                    DepartmentId = a.DepartmentId,
                    Department = a.Department.Name,
                    MobileNumber = a.MobileNumber,
                    DateOfBirth = a.DateOfBirth,
                    AddressLine = a.AddressLine,
                    Email = a.Email,
                    Photo = a.Photo
                })
                .ToListAsync();
        }

        private static AddressDto MapToDto(Address address)
        {
            var today = DateTime.Today;

            var age = today.Year - address.DateOfBirth.Year;

            if (address.DateOfBirth.Date >
                today.AddYears(-age))
            {
                age--;
            }

            return new AddressDto
            {
                Id = address.Id,
                FullName = address.FullName,
                JobId = address.JobId,
                JobTitle = address.Job.Name,
                DepartmentId = address.DepartmentId,
                Department = address.Department.Name,
                MobileNumber = address.MobileNumber,
                DateOfBirth = address.DateOfBirth,
                Age = age,
                AddressLine = address.AddressLine,
                Email = address.Email,
                Photo = address.Photo
            };
        }

        public async Task<byte[]> ExportToExcelAsync()
        {
            var addresses = await GetAllAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Addresses");
            var currentRow = 1;

            // Headers
            worksheet.Cell(currentRow, 1).Value = "Full Name";
            worksheet.Cell(currentRow, 2).Value = "Job Title";
            worksheet.Cell(currentRow, 3).Value = "Department";
            worksheet.Cell(currentRow, 4).Value = "Mobile Number";
            worksheet.Cell(currentRow, 5).Value = "Date of Birth";
            worksheet.Cell(currentRow, 6).Value = "Age";
            worksheet.Cell(currentRow, 7).Value = "Address";
            worksheet.Cell(currentRow, 8).Value = "Email";

            // Data
            foreach (var address in addresses)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = address.FullName;
                worksheet.Cell(currentRow, 2).Value = address.JobTitle;
                worksheet.Cell(currentRow, 3).Value = address.Department;
                worksheet.Cell(currentRow, 4).Value = address.MobileNumber;
                worksheet.Cell(currentRow, 5).Value = address.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cell(currentRow, 6).Value = address.Age;
                worksheet.Cell(currentRow, 7).Value = address.AddressLine;
                worksheet.Cell(currentRow, 8).Value = address.Email;
            }

            // Auto fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

    }
}
