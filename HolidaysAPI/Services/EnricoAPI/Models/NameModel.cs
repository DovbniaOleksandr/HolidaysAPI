using Newtonsoft.Json;

namespace HolidaysAPI.Services.EnricoAPI.Models
{
    public class NameModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
