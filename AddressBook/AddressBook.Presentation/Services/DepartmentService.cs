using AddressBook.Business.DTOs.Department;
using AddressBook.Business.Interfaces;
using AddressBook.Domain.Entities;
using AddressBook.Presentation.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Presentation.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            return await _context.Departments.AsNoTracking()
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            return await _context.Departments.AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            var exists = await _context.Departments.AnyAsync(d => d.Name == dto.Name);

            if (exists)
                throw new InvalidOperationException("Department already exists.");

            var department = new Department
            {
                Name = dto.Name
            };

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return false;

            var nameExists = await _context.Departments.AnyAsync(d => d.Id != id && d.Name == dto.Name);

            if (nameExists)
                throw new InvalidOperationException("Department already exists.");

            department.Name = dto.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return false;

            var isUsed = await _context.Addresses.AnyAsync(a => a.DepartmentId == id);

            if (isUsed)
                throw new InvalidOperationException("Cannot delete department because it is used by an address.");

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}