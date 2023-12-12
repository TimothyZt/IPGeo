using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using Microsoft.EntityFrameworkCore;
using IpToGeo.Utilities;

namespace IpToGeo.Services
{
    public class MysqlIpGeoService : IIpGeoService
    {
        private readonly IpToGeoDbContext _ipToGeoDbContext;
        private readonly IDataSourceService _dataSource;

        public MysqlIpGeoService(IpToGeoDbContext context, IDataSourceService dataSource)
        {
            _ipToGeoDbContext = context;
            _dataSource = dataSource;
        }

        public async Task UpdateIpGeoDataAsync()
        {
            await DeleteIpGeoTempTableAsync();
            await CreateTempTableAsync();
            await GetAndInsertDataAsync();
            await DeleteIpGeoTableAsync();
            await AlterTempTableNameAsync();
            await _ipToGeoDbContext.SaveChangesAsync();
        }

        protected async Task GetAndInsertDataAsync()
        {
            var source = await _dataSource.GetDataSource();
            var data = source.Select(a => (IpGeoTemp)a);
            await _ipToGeoDbContext.BulkInsertAsync(data);
        }

        public async Task<GeoliteCityIpv4Int?> GetIpv4GeoInfoAsync(string anyIp)
        {
            var s = IpFormatter.Ipv4ToNum(anyIp);
            var innerSelect = _ipToGeoDbContext.IpToGeo
                .OrderByDescending(m => m.IpRangeStart)
                .Where(a => s >= a.IpRangeStart)
                .Take(1);
            return await innerSelect.Where(c => s <= c.IpRangeEnd).SingleOrDefaultAsync();
        }

        protected async Task CreateTempTableAsync()
        {
            await _ipToGeoDbContext.Database.ExecuteSqlRawAsync(
                "CREATE TABLE `IpToGeoTemp`  (\n" +
                "`IpRangeStart` int UNSIGNED NOT NULL,\n" +
                "`IpRangeEnd` int UNSIGNED NOT NULL,\n" +
                "`CountryCode` text  ,\n" +
                " `State1` text  ,\n" +
                "`State2` text  ,\n" +
                "`City` text  ,\n" +
                "`Postcode` text  ,\n" +
                "`Latitude` text  ,\n" +
                "`Longitude` text ,\n" +
                "`Timezone` text  ,\n" +
                "INDEX ip_search (IpRangeStart DESC)\n" +
                ") ");
        }

        protected async Task AlterTempTableNameAsync() => await _ipToGeoDbContext.Database.ExecuteSqlRawAsync("ALTER TABLE IpToGeoTemp RENAME TO IpToGeo;");

        protected async Task DeleteIpGeoTableAsync() => await _ipToGeoDbContext.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS `IpToGeo`;");
        protected async Task DeleteIpGeoTempTableAsync() => await _ipToGeoDbContext.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS `IpToGeoTemp`;");
    }
}
