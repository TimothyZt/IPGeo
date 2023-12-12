using IpToGeo.Models;
using IpToGeo.Utilities;

namespace IpToGeo.Dtos
{
    public class IpLocationDbSourceDto
    {
        public string IpRangeStart { get; set; } = null!;

        public string IpRangeEnd { get; set; } = null!;

        public string CountryCode { get; set; } = null!;

        public string State1 { get; set; } = null!;

        public string State2 { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Postcode { get; set; } = null!;

        public string Latitude { get; set; } = null!;

        public string Longitude { get; set; } = null!;

        public string Timezone { get; set; } = null!;

        
        public static explicit operator IpGeoTmp(IpLocationDbSourceDto a) => new IpGeoTmp()
        {
            IpRangeStart = IpFormatter.Ipv4ToNum(a.IpRangeStart),
            IpRangeEnd = IpFormatter.Ipv4ToNum(a.IpRangeEnd),
            CountryCode = a.CountryCode,
            State1 = a.State1,
            State2 = a.State2,
            City = a.City,
            Postcode = a.Postcode,
            Latitude = a.Latitude,
            Longitude = a.Longitude,
            Timezone = a.Timezone,
        };
    }
}
