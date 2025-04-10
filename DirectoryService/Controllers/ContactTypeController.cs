using DirectoryService.DTOs.ContactInfo;
using DirectoryService.DTOs.ContactType;
using DirectoryService.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ContactTypeController : ControllerBase
    {
        private readonly IContactTypeService _contactTypeService;

        public ContactTypeController(IContactTypeService contactTypeService)
        {
            _contactTypeService = contactTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactTypeService.GetAllContactTypesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactTypeDTO contactTypeDto)
        {
            var result = await _contactTypeService.CreateContactTypeAsync(contactTypeDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contactTypeService.DeleteContactTypeAsync(id);
            return Ok(result);
        }
    }
}
