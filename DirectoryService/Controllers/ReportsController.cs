using DirectoryService.DTOs.Report;
using DirectoryService.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ReportsController : ControllerBase
    {
        private readonly IKafkaProducerService _producerService;
        public ReportsController(IKafkaProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestReport([FromBody] ReportRequestDTO request)
        {
            await _producerService.SendReportRequestAsync(request);
            return Ok("Report request sent.");
        }
    }
}
