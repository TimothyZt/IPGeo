using IpToGeo.Models;
using IpToGeo.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IpToGeo.Services
{
    public class MongoIpGeoService : IIpGeoService
    {
        private readonly IDataSourceService _dataSource;
        private readonly IMongoCollection<GeoliteCityIpv4Int> _mongoIpGeoService;
        private readonly IMongoDatabase _mongoDatabase;
        public MongoIpGeoService(IOptions<IpToGeoMongoDatabaseSettings> mongoIpGeoService, IDataSourceService dataSource)
        {
            var mongoClient = new MongoClient(mongoIpGeoService.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoIpGeoService.Value.DatabaseName);
            _mongoIpGeoService = _mongoDatabase.GetCollection<GeoliteCityIpv4Int>(mongoIpGeoService.Value.IpToGeosCollectionName);

            _dataSource = dataSource;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="anyIp"></param>
        /// <returns></returns>
        public async Task<GeoliteCityIpv4Int?> GetIpv4GeoInfoAsync(string anyIp)
        {
            var ip = IpFormatter.Ipv4ToNum(anyIp);
            var filter = Builders<GeoliteCityIpv4Int>.Filter.Lte("IpRangeStart", ip);
            var sort = Builders<GeoliteCityIpv4Int>.Sort.Descending(m => m.IpRangeStart);
            var options = new FindOptions<GeoliteCityIpv4Int, GeoliteCityIpv4Int>
            {
                Sort = sort,
                Limit = 1
            };
            var result = await _mongoIpGeoService.FindAsync(filter, options);
            var res = result.FirstOrDefault();
            if (res == null) return null;
            if (res.IpRangeEnd >= ip) return res;
            return null;
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIpGeoDataAsync()
        {
            await DeleteCollection("IpToGeosTemp");
            await CreateTempCollection();
            await GetAndInsertDataAsync();
            await DeleteCollection("IpToGeos");
            await RenameCollection();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        protected async Task GetAndInsertDataAsync()
        {
            var source = await _dataSource.GetDataSource();
            var data = source.Select(a => (IpGeoTemp)a);
            await InsertAsync(data);
        }

        /// <summary>
        /// 分块插入
        /// </summary>
        /// <param name="newIpToGeos"></param>
        /// <returns></returns>
        protected async Task InsertAsync(IEnumerable<IpGeoTemp> newIpToGeos)
        {
            var chunks = newIpToGeos.Chunk(100000);
            foreach (var chunk in chunks)
            {
                await _mongoDatabase.GetCollection<IpGeoTemp>("IpToGeosTemp").InsertManyAsync(chunk);
            }
        }

        protected async Task CreateTempCollection() => await _mongoDatabase.CreateCollectionAsync("IpToGeosTemp");

        protected async Task DeleteCollection(string dropCollectionName) => await _mongoDatabase.DropCollectionAsync(dropCollectionName);

        protected async Task RenameCollection() => await _mongoDatabase.RenameCollectionAsync("IpToGeosTemp", "IpToGeos");
    }
}
