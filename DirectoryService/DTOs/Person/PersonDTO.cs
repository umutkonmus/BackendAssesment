namespace DirectoryService.DTOs.Person
{
    public sealed class PersonDTO
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
    }
}
