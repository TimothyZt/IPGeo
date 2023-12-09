using IpToGeo.Models;
using Microsoft.EntityFrameworkCore;

namespace IpToGeo.IpToCityDbContext
{
    public class IpToGeoDbContext : DbContext
    {
        public IpToGeoDbContext(DbContextOptions<IpToGeoDbContext> options) : base(options)
        {
        }
        public DbSet<GeoliteCityIpv4Int> ipToGeoCity { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GeoliteCityIpv4Int>(builder =>
            {
                builder.HasNoKey();
                builder.ToTable("ipToGeoCity");
            });
        }
    }
}
