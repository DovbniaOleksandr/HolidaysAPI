using HolidaysAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysAPI.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }
    }
}
