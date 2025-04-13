namespace ReportService.DTOs.Report
{
    public sealed class ReportWithDetailsDTO
    {
        public Guid ID { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; }
        public List<ReportDetailDTO> ReportDetails { get; set; }
    }
}
