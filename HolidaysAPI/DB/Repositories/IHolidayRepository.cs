using HolidaysAPI.Models.Entities;

namespace HolidaysAPI.DB.Repositories
{
    public interface IHolidayRepository
    {
        Task<List<Holiday>> GetHolidaysByCountryAndYearAsync(string countryCode, int year);
        Task AddHolidaysAsync(List<Holiday> holidays);
        Task<List<Holiday>> GetHolidaysByDateAsync(DateTime date);
    }
}
