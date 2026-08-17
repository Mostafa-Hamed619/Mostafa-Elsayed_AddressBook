using AddressBook.Business.DTOs.JobTitle;
using AddressBook.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;

        public JobTitleController(
            IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobTitles =
                await _jobTitleService.GetAllAsync();

            return Ok(jobTitles);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var jobTitle =
                await _jobTitleService.GetByIdAsync(id);

            if (jobTitle == null)
                return NotFound();

            return Ok(jobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateJobTitleDto dto)
        {
            try
            {
                var jobTitle =
                    await _jobTitleService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = jobTitle.Id },
                    jobTitle);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = "Job title is already existing"
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateJobTitleDto dto)
        {
            try
            {
                var result = await _jobTitleService.UpdateAsync(id, dto);

                if (!result)
                    return NotFound();

                return Ok(new
                {
                    message = "Job title updated successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _jobTitleService.DeleteAsync(id);

                if (!result)
                    return NotFound();

                return Ok(new
                {
                    message = "Job title deleted successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}