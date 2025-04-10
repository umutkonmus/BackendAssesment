namespace DirectoryService.DTOs.ContactInfo
{
    public sealed class ContactInfoDTO
    {
        public Guid ID { get; set; }
        public Guid PersonID { get; set; }
        public Guid TypeID { get; set; }
        public string Value { get; set; }
    }
}
