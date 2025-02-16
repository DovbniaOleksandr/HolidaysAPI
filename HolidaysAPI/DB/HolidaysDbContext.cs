using HolidaysAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HolidaysAPI.DB
{
    public class HolidaysDbContext : DbContext
    {
        public HolidaysDbContext(DbContextOptions<HolidaysDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);
        }
    }
}
