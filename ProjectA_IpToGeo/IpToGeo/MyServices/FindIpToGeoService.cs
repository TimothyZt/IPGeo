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

        /// <summary>
        /// 查询IP地理位置
        /// </summary>
        /// <param name="anyIp"></param>
        /// <returns></returns>
        public GeoliteCityIpv4String GetAnyIp(string anyIp)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var s = IP_To_Num(anyIp);
            var innerSelect = _ipToGeoDbContext.ipToGeoCity.OrderByDescending(m => m.IpRangeStart).Where(a => s >= a.IpRangeStart).Take(1);
            var result = innerSelect.Where(c => s <= c.IpRangeEnd).ToList().Select(x => new GeoliteCityIpv4String
            {
                IpRangeStart = Int_To_Ip(x.IpRangeStart),
                IpRangeEnd = Int_To_Ip(x.IpRangeEnd),
                CountryCode = x.CountryCode,
                State1 = x.State1,
                State2 = x.State2,
                City = x.City,
                Postcode = x.Postcode,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Timezone = x.Timezone,
            }).SingleOrDefault();
            stopwatch.Stop();
            Console.WriteLine("单个ip查询运行时间：" + stopwatch.ElapsedMilliseconds);
            return result;
        }

        /// <summary>
        /// IP变十进制整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public uint IP_To_Num(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        /// <summary>
        /// 十进制整数变Ip
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public string Int_To_Ip(uint? ipInt)
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
