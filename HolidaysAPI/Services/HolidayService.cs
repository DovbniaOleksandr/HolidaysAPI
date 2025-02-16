using HolidaysAPI.DB.Repositories;
using HolidaysAPI.Models.Entities;
using HolidaysAPI.Models.Enums;
using HolidaysAPI.Services.EnricoAPI;
using System.Linq;

namespace HolidaysAPI.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IEnricoApiConnector _enricoConnector;

        public HolidayService(IEnricoApiConnector enricoConnector, IHolidayRepository holidayRepository)
        {
            _enricoConnector = enricoConnector;
            _holidayRepository = holidayRepository;
        }

        public async Task<DayStatus> GetDateStatusAsync(DateTime date)
        {
            var holidays = await _holidayRepository.GetHolidaysByDateAsync(date);

            if(!holidays.Any())
            {
                var isPublicHoliday = (await _enricoConnector.WhereIsPublicHolidayAsync(date)).Any();

                return isPublicHoliday
                    ? DayStatus.Holiday
                    : DateIsFreeDay(date)
                        ? DayStatus.FreeDay
                        : DayStatus.WorkDay;
            }

            return DayStatus.Holiday;
        }

        public async Task<IEnumerable<IGrouping<int, Holiday>>> GetHolidaysGroupedByMonthAsync(string countryCode, int year)
        {
            var holidays = await _holidayRepository.GetHolidaysByCountryAndYearAsync(countryCode, year);

            if(!holidays.Any())
            {
                holidays = (await _enricoConnector.GetHolidaysAsync(countryCode, year)).ToList();
                await _holidayRepository.AddHolidaysAsync(holidays);
            }

            return holidays
                .GroupBy(h => h.Date.Month)
                .OrderBy(g => g.Key);
        }

        public async Task<int> GetMaxConsecutiveFreeDays(string countryCode, int year)
        {
            var holidays = await _holidayRepository.GetHolidaysByCountryAndYearAsync(countryCode, year);

            if (!holidays.Any())
            {
                holidays = (await _enricoConnector.GetHolidaysAsync(countryCode, year)).ToList();
                await _holidayRepository.AddHolidaysAsync(holidays);
            }

            var freeDays = holidays.Select(h => h.Date).ToList();

            for (var date = new DateTime(year, 1, 1); date.Year == year; date = date.AddDays(1))
            {
                if (DateIsFreeDay(date))
                {
                    freeDays.Add(date);
                }
            }

            freeDays = freeDays.Distinct().OrderBy(d => d).ToList();

            int maxStreak = 0;
            int currentStreak = 1;

            for (int i = 1; i < freeDays.Count; i++)
            {
                if (freeDays[i].Date - freeDays[i - 1].Date == TimeSpan.FromDays(1))
                {
                    currentStreak++;
                }
                else
                {
                    maxStreak = Math.Max(maxStreak, currentStreak);
                    currentStreak = 1;
                }
            }

            maxStreak = Math.Max(maxStreak, currentStreak);

            return maxStreak;
        }

        public bool DateIsFreeDay(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday
                ? true
                : false;
        }
    }
}
