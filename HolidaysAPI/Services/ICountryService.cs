using HolidaysAPI.Models.Entities;

namespace HolidaysAPI.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
    }
}
