using AutoMapper;
using ReportService.DTOs.Report;
using ReportService.Models;

namespace ReportService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Report, ReportDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Report, ReportWithDetailsDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<ReportDetail, ReportDetailDTO>()
                .ReverseMap();
        }
    }
}
