using ReportService.Models.Abstract;
using ReportService.Utils;

namespace ReportService.Models
{
    public class Report : IEntity
    {
        public Guid ID { get; set; }
        public DateTime RequestedAt { get; set; } 
        public ReportStatus Status { get; set; } 
        public virtual List<ReportDetail> ReportDetails { get; set; }
    }
}
