using HolidaysAPI.Models.Entities;

namespace HolidaysAPI.Services.EnricoAPI
{
    public interface IEnricoApiConnector
    {
        Task<IEnumerable<Country>> GetSupportedCountriesAsync();
        Task<IEnumerable<Holiday>> GetHolidaysAsync(string countryCode, int year);
        Task<IEnumerable<EnricoAPI.Models.CountryModel>> WhereIsPublicHolidayAsync(DateTime date);
    }
}
