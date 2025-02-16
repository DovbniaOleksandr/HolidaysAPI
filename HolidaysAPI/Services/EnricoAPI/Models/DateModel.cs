using Newtonsoft.Json;

namespace HolidaysAPI.Services.EnricoAPI.Models
{
    public class DateModel
    {
        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("dayOfWeek")]
        public int DayOfWeek { get; set; }
    }
}
