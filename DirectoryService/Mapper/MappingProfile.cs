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

            CreateMap<Person, PersonWithContactInfoDTO>()
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.ContactInfos));
            #endregion

            #region ContactInfo
            CreateMap<ContactInfo, ContactInfoDTO>().ReverseMap();
            CreateMap<ContactInfo, CreateContactInfoDTO>().ReverseMap();

            // Bu mapping, ContactInfo içindeki Type.Name'i TypeName olarak mapler.
            CreateMap<ContactInfo, ContactInfoForPersonDTO>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name));
            #endregion

            #region ContactType
            CreateMap<ContactType, ContactTypeDTO>().ReverseMap();
            CreateMap<ContactType, CreateContactTypeDTO>().ReverseMap();
            #endregion
        }
    }
}
