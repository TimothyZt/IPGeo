using IpToGeo.Models;
using Microsoft.EntityFrameworkCore;

namespace IpToGeo.IpToCityDbContext
{
    public class IpToGeoDbContext : DbContext
    {
        public IpToGeoDbContext(DbContextOptions<IpToGeoDbContext> options) : base(options)
        {
        }
        public DbSet<GeoliteCityIpv4Int> IpToGeo { get; set; }
        public DbSet<IpGeoTmp> IpToGeoTmp { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GeoliteCityIpv4Int>(builder =>
            {
                builder.HasNoKey();
                builder.ToTable("IpToGeo");
            });
            builder.Entity<IpGeoTmp>(builder =>
            {
                builder.HasNoKey();
                builder.ToTable("IpToGeoTmp");
            });

        }
    }
}
