using CsvHelper.Configuration.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IpToGeo.Models
{
    public class GeoliteCityIpv4Int
    {
        [BsonRepresentation(BsonType.Int64)]
        [Name("ip_range_start")]
        [BsonId]
        public uint? IpRangeStart { get; set; }

        [BsonRepresentation(BsonType.Int64)]
        [Name("ip_range_end")]
        public uint? IpRangeEnd { get; set; }

        [Name("country_code")]
        public string? CountryCode { get; set; }

        [Name("state1")]
        public string? State1 { get; set; }

        [Name("state2")]
        public string? State2 { get; set; }

        [Name("city")]
        public string? City { get; set; }

        [Name("postcode")]
        public string? Postcode { get; set; }

        [Name("latitude")]
        public string? Latitude { get; set; }

        [Name("longitude")]
        public string? Longitude { get; set; }

        [Name("timezone")]
        public string? Timezone { get; set; }
    }
}
