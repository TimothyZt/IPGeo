using IpToGeo.Models;

namespace IpToGeo.Services
{
    public class MongoIpGeoService : IIpGeoService
    {
        public Task<GeoliteCityIpv4Int?> GetIpv4GeoInfoAsync(string anyIp)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIpGeoDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
