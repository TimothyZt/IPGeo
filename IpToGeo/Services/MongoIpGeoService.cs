using Bogus;
using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using IpToGeo.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System.Diagnostics;

namespace IpToGeo.Services
{
    public class MongoIpGeoService : IIpGeoService
    {
       
        private readonly IDataSourceService _dataSource;
        private readonly IMongoCollection<GeoliteCityIpv4Int> _mongoIpGeoService;
        public MongoIpGeoService(IOptions<IpToGeoMongoDatabaseSettings> mongoIpGeoService, IDataSourceService dataSource)
        {
            var mongoClient = new MongoClient(mongoIpGeoService.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoIpGeoService.Value.DatabaseName);
            _mongoIpGeoService = mongoDatabase.GetCollection<GeoliteCityIpv4Int>(mongoIpGeoService.Value.IpToGeosCollectionName);
            _dataSource = dataSource;
        }     
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="anyIp"></param>
        /// <returns></returns>
        public async Task<GeoliteCityIpv4Int?> GetIpv4GeoInfoAsync(string anyIp)
        {
            GeoliteCityIpv4Int empty = null;
            var ip = IpFormatter.Ipv4ToNum(anyIp);
            var filter = Builders<GeoliteCityIpv4Int>.Filter.Lte("IpRangeStart", ip);
           Trace.WriteLine(filter.ToString());
            var sort = Builders<GeoliteCityIpv4Int>.Sort.Descending(m => m.IpRangeStart);
            var options = new FindOptions<GeoliteCityIpv4Int, GeoliteCityIpv4Int>
            {
                Sort = sort,
                Limit = 1
            };
            var result = await _mongoIpGeoService.FindAsync(filter, options);
            var res = result.FirstOrDefault();
            if (res == null) return empty;
            if (res.IpRangeEnd>ip) return res;
            return empty;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIpGeoDataAsync()
        {
            await DeleteCollection("IpToGeosTemp");
            CreateTempCollection();
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
        /// 插入
        /// </summary>
        /// <param name="newIpToGeos"></param>
        /// <returns></returns>
        protected  async Task InsertAsync(IEnumerable<IpGeoTemp> newIpToGeos)
        {
            var _server = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            var db = _server.GetDatabase("IpToGeoMongo");
            var chunks = newIpToGeos.Chunk(100000);
            foreach (var chunk in chunks)
            {
                await db.GetCollection<IpGeoTemp>("IpToGeosTemp").InsertManyAsync(chunk);
            }
        }
        /// <summary>
        /// 创建集合
        /// </summary>
        protected async void CreateTempCollection() 
        {
            var _server = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            var db = _server.GetDatabase("IpToGeoMongo");
            await db.CreateCollectionAsync("IpToGeosTemp");
        }
        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="dropCollectionName"></param>
        /// <returns></returns>
        protected async Task DeleteCollection(string dropCollectionName) 
        {
            var _server = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            var db = _server.GetDatabase("IpToGeoMongo");
            await db.DropCollectionAsync(dropCollectionName);
        }
        /// <summary>
        /// 集合改名
        /// </summary>
        /// <returns></returns>
        protected async Task RenameCollection()
        {
            var _server = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            var db = _server.GetDatabase("IpToGeoMongo");
            await db.RenameCollectionAsync("IpToGeosTemp", "IpToGeos");  
        }
    }
}
