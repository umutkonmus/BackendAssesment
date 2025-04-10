using DirectoryService.DTOs.ContactType;
using DirectoryService.Utils.Response;

namespace DirectoryService.Services.Abstracts
{
    public interface IContactTypeService
    {
        Task<Response<CreateContactTypeDTO>> CreateContactTypeAsync(CreateContactTypeDTO contactType);
        Task<Response<bool>> DeleteContactTypeAsync(Guid id);
        Task<Response<List<ContactTypeDTO>>> GetAllContactTypesAsync();
    }
}
