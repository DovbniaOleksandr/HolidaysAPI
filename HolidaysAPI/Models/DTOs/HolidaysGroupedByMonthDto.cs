using HolidaysAPI.Models.Entities;

namespace HolidaysAPI.Models.DTOs
{
    public class HolidaysGroupedByMonthDto
    {
        public int Month { get; set; }
        public List<HolidayDto> Holidays { get; set; }
    }
}
