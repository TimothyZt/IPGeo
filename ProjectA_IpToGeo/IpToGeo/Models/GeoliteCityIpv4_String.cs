using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IpToGeo.Models
{
    public class GeoliteCityIpv4_String
    {
        [Name("ip_range_start")]

        public string? ip_range_start { get; set; }
        [Name("ip_range_end")]

        public string? ip_range_end { get; set; }
        [Name("country_code")]

        public string? country_code { get; set; }
        [Name("state1")]

        public string? state1 { get; set; }
        [Name("state2")]

        public string? state2 { get; set; }
        [Name("city ")]

        public string? city { get; set; }
        [Name("postcode")]

        public string? postcode { get; set; }
        [Name("latitude")]

        public string? latitude { get; set; }
        [Name("longitude")]

        public string? longitude { get; set; }
        [Name("timezone")]

        public string? timezone { get; set; }
    }
}
