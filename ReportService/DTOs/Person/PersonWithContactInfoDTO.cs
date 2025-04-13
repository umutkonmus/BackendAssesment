namespace ReportService.DTOs.Person
{
    public sealed class PersonWithContactInfoDTO : PersonDTO
    {
        public List<ContactInfoDTO> ContactInfos { get; set; }
    }
}
