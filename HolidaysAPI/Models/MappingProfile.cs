using AutoMapper;
using HolidaysAPI.Models.DTOs;
using HolidaysAPI.Models.Entities;

namespace HolidaysAPI.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Holiday, HolidayDto>().ReverseMap();
        }
    }
}
