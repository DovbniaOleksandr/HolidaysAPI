using HolidaysAPI.Models.Entities;
using HolidaysAPI.Models.Enums;

namespace HolidaysAPI.Services
{
    public interface IHolidayService
    {
        Task<IEnumerable<IGrouping<int, Holiday>>> GetHolidaysGroupedByMonthAsync(string countryCode, int year);
        Task<DayStatus> GetDateStatusAsync(DateTime date);
        Task<int> GetMaxConsecutiveFreeDays(string countryCode, int year);
    }
}
