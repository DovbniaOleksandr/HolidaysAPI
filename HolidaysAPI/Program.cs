using HolidaysAPI.DB;
using HolidaysAPI.DB.Repositories;
using HolidaysAPI.Extensions;
using HolidaysAPI.Services;
using HolidaysAPI.Services.EnricoAPI;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HolidaysDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HolidaysDBConnection")));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IEnricoApiConnector, EnricoApiConnector>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<HolidaysDbContext>();
        dbContext.Database.Migrate();
    }
}

app.Run();

public partial class Program { }
