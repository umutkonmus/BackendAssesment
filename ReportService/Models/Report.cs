using ReportService.Utils;

namespace ReportService.Models
{
    public sealed class Report
    {
        public Guid ID { get; set; }
        public DateTime RequestedAt { get; set; } 
        public ReportStatus Status { get; set; } 
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
