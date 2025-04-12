using DirectoryService.DTOs.Report;
using DirectoryService.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportContoller : ControllerBase
    {
        private readonly IKafkaProducerService _producerService;
        public ReportContoller(IKafkaProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestReport([FromBody] ReportRequestDTO request)
        {
            await _producerService.SendReportRequestAsync(request);
            return Ok("Rapor talebi gönderildi.");
        }
    }
}
