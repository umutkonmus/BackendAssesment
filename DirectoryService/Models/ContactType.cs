using DirectoryService.Models.Abstract;

namespace DirectoryService.Models
{
    public sealed class ContactType : IEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
