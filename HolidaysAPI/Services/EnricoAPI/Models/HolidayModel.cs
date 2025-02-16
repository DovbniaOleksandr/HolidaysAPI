using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace HolidaysAPI.Services.EnricoAPI.Models
{
    public class HolidayModel
    {
        [JsonProperty("date")]
        public DateModel Date { get; set; }

        [JsonProperty("name")]
        public List<NameModel> Names { get; set; }
    }
}
