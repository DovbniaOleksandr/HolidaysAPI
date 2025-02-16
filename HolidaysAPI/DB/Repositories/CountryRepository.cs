using HolidaysAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HolidaysAPI.DB.Repositories
{
    public class CountryRepository: ICountryRepository
    {
        private readonly HolidaysDbContext _context;

        public CountryRepository(HolidaysDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task AddCountriesAsync(IEnumerable<Country> countries)
        {
            var newCountries = countries
                .Where(c => !_context.Countries.Any(dbC => dbC.Code == c.Code))
                .ToList();

            if (newCountries.Any())
            {
                await _context.Countries.AddRangeAsync(newCountries);
                await _context.SaveChangesAsync();
            }
        }
    }
}
