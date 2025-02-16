using HolidaysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HolidaysAPI.DB.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly HolidaysDbContext _dbContext;
        public HolidayRepository(HolidaysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Holiday>> GetHolidaysByCountryAndYearAsync(string countryCode, int year)
        {
            return await _dbContext.Holidays
                .Where(h => h.CountryCode == countryCode && h.Date.Year == year)
                .ToListAsync();
        }

        public async Task AddHolidaysAsync(List<Holiday> holidays)
        {
            _dbContext.Holidays.AddRange(holidays);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Holiday>> GetHolidaysByDateAsync(DateTime date)
        {
            return await _dbContext.Holidays
                .Where(h => h.Date == date)
                .ToListAsync();
        }
    }
}
