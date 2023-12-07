using IpToGeo.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml;

namespace IpToGeo.IpToCityDbContext
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        //泛型
        public DbSet<GeoliteCityIpv4_Int> ipToGeoCity { get; set; }//泛型

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //从当前程序集命名空间加载所有的IEntityTypeConfiguration
            builder.Entity<GeoliteCityIpv4_Int>(builder =>
            {
                builder.HasNoKey();
                builder.ToTable("ipToGeoCity");
            });
        }
    }
}
