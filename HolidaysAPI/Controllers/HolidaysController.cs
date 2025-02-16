using HolidaysAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysAPI.Controllers
{
    [Route("api/holidays")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidaysController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet("grouped-by-month")]
        public async Task<ActionResult<IEnumerable<object>>> GetGroupedHolidaysByMonth([FromQuery] string countryCode, [FromQuery] int year)
        {
            var groupedHolidays = await _holidayService.GetHolidaysGroupedByMonthAsync(countryCode, year);

            return Ok(groupedHolidays.Select(g => new
            {
                Month = g.Key,
                Holidays = g.Select(h => new
                {
                    h.Id,
                    h.Name,
                    Date = h.Date.ToString("yyyy-MM-dd")
                })
            }));
        }

        [HttpGet("get-date-status")]
        public async Task<ActionResult<IEnumerable<object>>> GetDateStatus([FromQuery] DateTime date)
        {
            var dateStatus = await _holidayService.GetDateStatusAsync(date);

            return Ok(dateStatus);
        }

        [HttpGet("get-max-consecutive-free-days")]
        public async Task<ActionResult<IEnumerable<object>>> GetMaxConsecutiveFreeDays([FromQuery] string countryCode, [FromQuery] int year)
        {
            var maxStreak = await _holidayService.GetMaxConsecutiveFreeDays(countryCode, year);

            return Ok(maxStreak);
        }
    }
}
