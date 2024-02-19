using Microsoft.EntityFrameworkCore;

namespace MusicBandAPI.Model
{
    public class CountryContext : DbContext
    {
        public DbSet<Country>? countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Country.db");
        }
    }
}
