using DirectoryService.DTOs.ContactInfo;

namespace DirectoryService.DTOs.Person
{
    public sealed class PersonWithContactInfoDTO
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public List<ContactInfoForPersonDTO> ContactInfos { get; set; }
    }
}
