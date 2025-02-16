using AutoMapper;
using HolidaysAPI.DB.Repositories;
using HolidaysAPI.Models;
using HolidaysAPI.Services.EnricoAPI;

namespace HolidaysAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IEnricoApiConnector _enricoConnector;

        public CountryService(IEnricoApiConnector enricoConnector, ICountryRepository countryRepository)
        {
            _enricoConnector = enricoConnector;
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();

            if (!countries.Any())
            {
                countries = await _enricoConnector.GetSupportedCountriesAsync();
                await _countryRepository.AddCountriesAsync(countries);
            }

            return countries;
        }
    }
}
