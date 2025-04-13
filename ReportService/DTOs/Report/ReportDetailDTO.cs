namespace ReportService.DTOs.Report
{
    public sealed class ReportDetailDTO
    {
        public Guid ID { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
