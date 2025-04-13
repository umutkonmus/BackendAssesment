namespace ReportService.Models
{
    public class ReportDetail
    {
        public Guid ID { get; set; }
        public Guid ReportID { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
        //virtual for ef lazy loading
        public virtual Report Report { get; set; }
    }
}
