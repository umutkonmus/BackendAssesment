using DirectoryService.Models.Abstract;

namespace DirectoryService.Models
{
    public sealed class Person : IEntity
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public List<ContactInfo> ContactInfos { get; set; }
    }
}
