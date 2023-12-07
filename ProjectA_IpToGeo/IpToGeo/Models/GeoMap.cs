using CsvHelper.Configuration;

namespace IpToGeo.Models
{
    public sealed class GeoMap:ClassMap<GeoliteCityIpv4_String>
    {
        public GeoMap()
        {
          
            Map(f => f.ip_range_start).Name("ip_range_start");
            Map(f => f.ip_range_end).Name("ip_range_end");
            Map(f => f.country_code).Name("country_code");
            Map(f => f.state1).Name("state1");
            Map(f => f.state2).Name("state2");
            Map(f => f.city).Name("city");
            Map(f => f.postcode).Name("postcode");
            Map(f => f.latitude).Name("latitude");
            Map(f => f.longitude).Name("longitude");
            Map(f => f.timezone).Name("timezone");
        }
    }
}
