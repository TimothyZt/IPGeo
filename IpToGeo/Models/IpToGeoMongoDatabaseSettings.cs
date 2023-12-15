namespace IpToGeo.Models
{
    public class IpToGeoMongoDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string IpToGeosCollectionName { get; set; } = null!;
    }
}
