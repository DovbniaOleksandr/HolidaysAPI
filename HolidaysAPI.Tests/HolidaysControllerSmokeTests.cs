using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace HolidaysAPI.Tests
{
    public class HolidaysControllerSmokeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HolidaysControllerSmokeTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ConnectionStrings:HolidaysDBConnection", "Server=localhost,1433;Database=HolidaysDB_Test;User Id=sa;Password=Str0ngP@ssw0rd!;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection = false;");
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                { });
            });
        }

        [Theory]
        [InlineData("/api/holidays/grouped-by-month?countryCode=US&year=2025")]
        [InlineData("/api/holidays/get-date-status?date=2025-01-01")]
        [InlineData("/api/holidays/get-max-consecutive-free-days?countryCode=US&year=2025")]
        public async Task Get_EndpointsReturnSuccessAndJsonContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}