using AutoMapper;
using HolidaysAPI.Models.DTOs;
using HolidaysAPI.Models.Entities;
using HolidaysAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysAPI.Controllers
{
    [Route("api/holidays")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IMapper _mapper;

        public HolidaysController(IHolidayService holidayService, IMapper mapper)
        {
            _holidayService = holidayService;
            _mapper = mapper;
        }

        [HttpGet("grouped-by-month")]
        public async Task<IActionResult> GetGroupedHolidaysByMonth([FromQuery] string countryCode, [FromQuery] int year)
        {
            var groupedHolidays = await _holidayService.GetHolidaysGroupedByMonthAsync(countryCode, year);

            var groupedHolidaysDto = groupedHolidays.Select(g => new HolidaysGroupedByMonthDto
                {
                    Month = g.Key,
                    Holidays = _mapper.Map<List<HolidayDto>>(g.ToList())
                });

            return Ok(groupedHolidaysDto);
        }

        [HttpGet("get-date-status")]
        public async Task<IActionResult> GetDateStatus([FromQuery] DateTime date)
        {
            var dateStatus = await _holidayService.GetDateStatusAsync(date);

            return Ok(dateStatus.ToString());
        }

        [HttpGet("get-max-consecutive-free-days")]
        public async Task<IActionResult> GetMaxConsecutiveFreeDays([FromQuery] string countryCode, [FromQuery] int year)
        {
            var maxStreak = await _holidayService.GetMaxConsecutiveFreeDays(countryCode, year);

            return Ok(maxStreak);
        }
    }
}
