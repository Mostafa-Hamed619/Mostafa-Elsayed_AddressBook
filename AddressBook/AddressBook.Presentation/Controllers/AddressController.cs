using AddressBook.Business.DTOs.Address;
using AddressBook.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressService.GetAllAsync();

            return Ok(addresses);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateAddressDto dto)
        {
            try
            {
                var address = await _addressService.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateAddressDto dto)
        {
            var result = await _addressService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _addressService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] AddressSearchDto dto)
        {
            var result = await _addressService.SearchAsync(dto);

            return Ok(result);
        }

        [HttpGet("export")]
        [Authorize]
        public async Task<IActionResult> Export()
        {
            var fileBytes = await _addressService.ExportToExcelAsync();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Addresses.xlsx");
        }
    }
}