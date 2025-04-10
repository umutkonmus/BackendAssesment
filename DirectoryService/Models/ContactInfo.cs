using DirectoryService.Models.Abstract;

namespace DirectoryService.Models
{
    public sealed class ContactInfo : IEntity
    {
        public Guid ID { get; set; }
        public Guid PersonID { get; set; }
        public Guid TypeID { get; set; }
        public string Value { get; set; }
        public Person Person { get; set; }
        public ContactType Type { get; set; }
    }
}
