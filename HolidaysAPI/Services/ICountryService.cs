using HolidaysAPI.Models;

namespace HolidaysAPI.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
    }
}
