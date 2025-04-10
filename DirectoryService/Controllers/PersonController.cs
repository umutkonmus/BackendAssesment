using DirectoryService.DTOs.Person;
using DirectoryService.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _personService.GetAllPersonsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _personService.GetPersonByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonDTO personDto)
        {
            var result = await _personService.CreatePersonAsync(personDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _personService.DeletePersonAsync(id);
            return Ok(result);
        }
    }
}
