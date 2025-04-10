using DirectoryService.DTOs.Person;
using DirectoryService.Utils.Response;
namespace DirectoryService.Services.Abstracts
{
    public interface IPersonService
    {
        Task<Response<CreatePersonDTO>> CreatePersonAsync(CreatePersonDTO person);
        Task<Response<bool>> DeletePersonAsync(Guid id);
        Task<Response<List<PersonDTO>>> GetAllPersonsAsync();
        Task<Response<PersonWithContactInfoDTO>> GetPersonByIdAsync(Guid id);
    }
}
