using AutoMapper;
using DirectoryService.DTOs.ContactInfo;
using DirectoryService.DTOs.ContactType;
using DirectoryService.DTOs.Person;
using DirectoryService.Models;

namespace DirectoryService.Mapper
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Person
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<Person, CreatePersonDTO>().ReverseMap();
            CreateMap<Person, PersonWithContactInfoDTO>().ReverseMap();
            #endregion
            #region ContactInfo
            CreateMap<ContactInfo, ContactInfoDTO>().ReverseMap();
            CreateMap<ContactInfo, CreateContactInfoDTO>().ReverseMap();
            #endregion
            #region ContactType
            CreateMap<ContactType, ContactTypeDTO>().ReverseMap();
            CreateMap<ContactType, CreateContactTypeDTO>().ReverseMap();
            #endregion
        }
    }
}
