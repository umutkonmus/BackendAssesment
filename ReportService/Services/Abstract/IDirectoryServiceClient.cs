using ReportService.DTOs.Person;

namespace ReportService.Services.Abstract
{
    public interface IDirectoryServiceClient
    {
        Task<List<PersonDTO>> GetAllPersonsAsync();
        Task<List<ContactInfoDTO>> GetAllContactInfosAsync();
    }
}
