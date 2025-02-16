using AutoMapper;
using HolidaysAPI.Models.DTOs;
using HolidaysAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysAPI.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountriesController(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();

            var countriesDto = _mapper.Map<IEnumerable<CountryDto>>(countries);

            return Ok(countriesDto);
        }
    }
}
