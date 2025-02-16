using HolidaysAPI.Services.EnricoAPI.Models;
using Newtonsoft.Json;
using Country = HolidaysAPI.Models.Entities.Country;
using Holiday = HolidaysAPI.Models.Entities.Holiday;

namespace HolidaysAPI.Services.EnricoAPI
{
    public class EnricoApiConnector : IEnricoApiConnector
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IEnricoApiConnector> _logger;
        private const string BaseUrl = "https://kayaposoft.com/enrico/json/v2.0/";

        public EnricoApiConnector(IHttpClientFactory httpClientFactory, ILogger<IEnricoApiConnector> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Country>> GetSupportedCountriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{BaseUrl}?action=getSupportedCountries");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<CountryModel>>(json);
                return countries?.Select(c => new Country
                    {
                        Name = c.FullName,
                        Code = c.CountryCode
                    }) 
                    ?? new List<Country>();
            }

            _logger.LogError($"Failed to fetch countries: {response.StatusCode}");
            return new List<Country>();
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysAsync(string countryCode, int year)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{BaseUrl}?action=getHolidaysForYear&year={year}&country={countryCode}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var holidays = JsonConvert.DeserializeObject<List<HolidayModel>>(json);
                return holidays?.Select(h => EnricoHolidayToDBHoliday(h, countryCode)) ?? new List<Holiday>();
            }

            _logger.LogError($"Failed to fetch holidays for {countryCode} in {year}: {response.StatusCode}");
            return new List<Holiday>();
        }

        public async Task<IEnumerable<CountryModel>> WhereIsPublicHolidayAsync(DateTime date)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{BaseUrl}?action=whereIsPublicHoliday&date={date.ToString("dd-MM-yyyy")}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CountryModel>>(json);
                return result ?? new List<CountryModel>();
            }

            _logger.LogError($"Failed to check day status on {date}: {response.StatusCode}");
            return new List<CountryModel>();
        }

        private Holiday EnricoHolidayToDBHoliday(HolidayModel holiday, string countryCode)
        {
            return new Holiday
            {
                CountryCode = countryCode,
                Name = holiday.Names.Where(n => n.Lang == "en").FirstOrDefault()?.Text ?? "Unknown",
                Date = new DateTime(holiday.Date.Year, holiday.Date.Month, holiday.Date.Day)
            };
        }
    }
}
