using IpToGeo.Models;

namespace IpToGeo.Services
{
    public interface IIpGeoService
    {
        /// <summary>
        /// Fetch and update all IP-Geo data
        /// </summary>
        /// <returns></returns>
        public Task UpdateIpGeoDataAsync();

        /// <summary>
        /// Get IP-Geo info of any ipv4.
        /// </summary>
        /// <param name="anyIp"></param>
        /// <returns>IP-Geo record instance if any. Null otherwise.</returns>
        public Task<GeoliteCityIpv4Int?> GetIpv4GeoInfoAsync(string anyIp);
    }
}
