using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.ContactInfo;
using DirectoryService.Utils.Response;

namespace DirectoryService.Services.Abstracts
{
    public interface IContactInfoService
    {
        Task<Response<CreateContactInfoDTO>> CreateContactInfoAsync(CreateContactInfoDTO contactInfo);
        Task<Response<bool>> DeleteContactInfoAsync(Guid id);
    }
}
