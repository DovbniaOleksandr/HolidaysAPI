using HolidaysAPI.DB.Repositories;
using HolidaysAPI.Models.Entities;
using HolidaysAPI.Models.Enums;
using HolidaysAPI.Services;
using HolidaysAPI.Services.EnricoAPI;
using HolidaysAPI.Services.EnricoAPI.Models;
using Moq;

namespace HolidaysAPI.Tests
{
    public class HolidayServiceTests
    {
        private readonly Mock<IHolidayRepository> _mockHolidayRepository;
        private readonly Mock<IEnricoApiConnector> _mockEnricoConnector;
        private readonly HolidayService _holidayService;

        public HolidayServiceTests()
        {
            _mockHolidayRepository = new Mock<IHolidayRepository>();
            _mockEnricoConnector = new Mock<IEnricoApiConnector>();
            _holidayService = new HolidayService(_mockEnricoConnector.Object, _mockHolidayRepository.Object);
        }

        [Fact]
        public async Task GetDateStatusAsync_ShouldReturnHoliday_WhenDateIsStoredAsHoliday()
        {
            var date = new DateTime(2025, 1, 1);
            _mockHolidayRepository.Setup(repo => repo.GetHolidaysByDateAsync(date))
                                  .ReturnsAsync(new List<Holiday> { new Holiday { Date = date } });

            var result = await _holidayService.GetDateStatusAsync(date);

            Assert.Equal(DayStatus.Holiday, result);
        }

        [Fact]
        public async Task GetDateStatusAsync_ShouldReturnHoliday_WhenEnricoApiReturnsPublicHoliday()
        {
            var date = new DateTime(2025, 1, 1);
            _mockHolidayRepository.Setup(repo => repo.GetHolidaysByDateAsync(date))
                                  .ReturnsAsync(new List<Holiday>());

            _mockEnricoConnector.Setup(connector => connector.WhereIsPublicHolidayAsync(date))
                                .ReturnsAsync(new List<CountryModel> { new CountryModel { CountryCode = "us" } });

            var result = await _holidayService.GetDateStatusAsync(date);

            Assert.Equal(DayStatus.Holiday, result);
        }

        [Fact]
        public async Task GetDateStatusAsync_ShouldReturnFreeDay_WhenDateIsWeekend()
        {
            var date = new DateTime(2025, 1, 4);
            _mockHolidayRepository.Setup(repo => repo.GetHolidaysByDateAsync(date))
                                  .ReturnsAsync(new List<Holiday>());

            _mockEnricoConnector.Setup(connector => connector.WhereIsPublicHolidayAsync(date))
                                .ReturnsAsync(new List<CountryModel>());

            var result = await _holidayService.GetDateStatusAsync(date);

            Assert.Equal(DayStatus.FreeDay, result);
        }

        [Fact]
        public async Task GetDateStatusAsync_ShouldReturnWorkDay_WhenDateIsNotHolidayOrFreeDay()
        {
            var date = new DateTime(2025, 1, 2);
            _mockHolidayRepository.Setup(repo => repo.GetHolidaysByDateAsync(date))
                                  .ReturnsAsync(new List<Holiday>());

            _mockEnricoConnector.Setup(connector => connector.WhereIsPublicHolidayAsync(date))
                                .ReturnsAsync(new List<CountryModel>());

            var result = await _holidayService.GetDateStatusAsync(date);

            Assert.Equal(DayStatus.WorkDay, result);
        }

        [Theory]
        [InlineData("2025-02-15", true)] // Saturday
        [InlineData("2025-02-16", true)] // Sunday
        [InlineData("2025-02-17", false)] // Monday
        [InlineData("2025-02-13", false)] // Thursday
        [InlineData("2025-02-14", false)] // Friday
        public void DateIsFreeDay_ReturnsExpectedResult(string dateString, bool expected)
        {
            // Arrange
            DateTime date = DateTime.Parse(dateString);

            // Act
            bool result = _holidayService.DateIsFreeDay(date);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}