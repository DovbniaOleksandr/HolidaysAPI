namespace HolidaysAPI.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string CountryCode { get; set; }
    }
}
