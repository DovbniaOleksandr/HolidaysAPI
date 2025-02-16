using HolidaysAPI.Models;

namespace HolidaysAPI.DB.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task AddCountriesAsync(IEnumerable<Country> countries);
    }
}
