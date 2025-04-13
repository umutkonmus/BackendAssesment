using ReportService.DTOs.Report;
using ReportService.Utils;

namespace ReportService.Services.Abstract
{
    public interface IReportService
    {
        Task<Response<List<ReportDTO>>> GetAllReportsAsync();
        Task<Response<ReportWithDetailsDTO>> GetReportByIdAsync(Guid id);
        Task<Response<ReportDTO>> CreateReportAsync(string location);
        Task<Response<ReportDTO>> ProcessReportAsync(Guid reportId, string location);
    }
}
