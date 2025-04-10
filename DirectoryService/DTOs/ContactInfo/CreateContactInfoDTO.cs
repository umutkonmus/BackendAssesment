namespace DirectoryService.DTOs.ContactInfo
{
    public sealed class CreateContactInfoDTO
    {
        public Guid PersonID { get; set; }
        public Guid TypeID { get; set; }
        public string Value { get; set; }
    }
}
