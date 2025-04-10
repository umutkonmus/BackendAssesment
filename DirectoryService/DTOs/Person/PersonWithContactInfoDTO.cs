using DirectoryService.Models;

namespace DirectoryService.DTOs.Person
{
    public sealed class PersonWithContactInfoDTO
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public List<Models.ContactInfo> ContactInfos { get; set; }
    }
}
