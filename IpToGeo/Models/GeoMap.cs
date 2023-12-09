using CsvHelper.Configuration;
using IpToGeo.Dtos;

namespace IpToGeo.Models
{
    public sealed class GeoMap:ClassMap<IpLocationDbSourceDto>
    {
        public GeoMap()
        {
          
            Map(f => f.IpRangeStart).Name("ip_range_start");
            Map(f => f.IpRangeEnd).Name("ip_range_end");
            Map(f => f.CountryCode).Name("country_code");
            Map(f => f.State1).Name("state1");
            Map(f => f.State2).Name("state2");
            Map(f => f.City).Name("city");
            Map(f => f.Postcode).Name("postcode");
            Map(f => f.Latitude).Name("latitude");
            Map(f => f.Longitude).Name("longitude");
            Map(f => f.Timezone).Name("timezone");
        }
    }
}
