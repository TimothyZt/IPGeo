using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using System.Diagnostics;
using System.Text;

namespace IpToGeo.MyServices
{
    public class FindIpToGeoService
    {
        private readonly IpToGeoDbContext _ipToGeoDbContext;
     
        public FindIpToGeoService(IpToGeoDbContext context) 
        {
            _ipToGeoDbContext = context;
        }

        public GeoliteCityIpv4_String GetAnyIp(string anyIp) 
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var s = IP_To_Num(anyIp);
            var innerSelect = _ipToGeoDbContext.ipToGeoCity.OrderByDescending(m => m.ip_range_start).Where(a => s >= a.ip_range_start).Take(1);
            var result = innerSelect.Where(c => s <= c.ip_range_end).ToList().Select(x => new GeoliteCityIpv4_String
            {
                ip_range_start = Int_To_Ip(x.ip_range_start),
                ip_range_end = Int_To_Ip(x.ip_range_end),
                country_code = x.country_code,
                state1 = x.state1,
                state2 = x.state2,
                postcode = x.postcode,
                latitude = x.latitude,
                longitude = x.longitude,
                timezone = x.timezone,
            }).SingleOrDefault();
            stopwatch.Stop();
            Console.WriteLine("单个ip查询运行时间：" + stopwatch.ElapsedMilliseconds);
            return result;
        }
        public ulong IP_To_Num(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return ulong.Parse(items[0]) << 24
                    | ulong.Parse(items[1]) << 16
                    | ulong.Parse(items[2]) << 8
                    | ulong.Parse(items[3]);
        }
        public string Int_To_Ip(ulong? ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
    }
}
