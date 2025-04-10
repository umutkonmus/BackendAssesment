using DirectoryService.DTOs.ContactInfo;
using DirectoryService.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfoController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactInfoDTO contactInfoDto)
        {
            var result = await _contactInfoService.CreateContactInfoAsync(contactInfoDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contactInfoService.DeleteContactInfoAsync(id);
            return Ok(result);
        }
    }
}
