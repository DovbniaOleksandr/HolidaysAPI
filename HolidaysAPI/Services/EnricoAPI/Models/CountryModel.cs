using System.Text.Json.Serialization;

namespace HolidaysAPI.Services.EnricoAPI.Models
{
    public class CountryModel
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }
}
