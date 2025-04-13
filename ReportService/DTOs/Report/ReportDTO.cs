using ReportService.Utils;

namespace ReportService.DTOs.Report
{
    public sealed class ReportDTO
    {
        public Guid ID { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; }
    }
}
