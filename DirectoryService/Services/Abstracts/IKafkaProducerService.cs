using DirectoryService.DTOs.Report;

namespace DirectoryService.Services.Abstracts
{
    public interface IKafkaProducerService
    {
        Task SendReportRequestAsync(ReportRequestDTO dto);
    }
}
