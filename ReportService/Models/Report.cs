namespace ReportService.Models
{
    public sealed class Report
    {
        public Guid ID { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }

    public enum ReportStatus
    {
        Preparing = 0,
        Completed = 1
    }
}
