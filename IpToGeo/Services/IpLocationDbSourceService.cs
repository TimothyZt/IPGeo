using CsvHelper.Configuration;
using CsvHelper;
using IpToGeo.Models;
using System.Globalization;
using System.IO.Compression;
using IpToGeo.Dtos;
using DnsClient.Protocol;
using System.Diagnostics;

namespace IpToGeo.Services
{
    /// <summary>
    /// IP-Geo database from https://github.com/sapics/ip-location-db
    /// </summary>
    public class IpLocationDbSourceService : IDataSourceService
    {
        private readonly HttpClient _httpClient;
        private readonly string downloadUrl = "https://raw.githubusercontent.com/sapics/ip-location-db/main/geolite2-city/geolite2-city-ipv4.csv.gz";

        public IpLocationDbSourceService(IHttpClientFactory factory) => _httpClient = factory.CreateClient();

        /// <summary>
        /// Fetch, download and process the data from ip-location-db.
        /// </summary>
        public async Task<IEnumerable<IpLocationDbSourceDto>> GetDataSource()
        {
            // timeout only for reading header
            // https://stackoverflow.com/questions/29851491/how-exactly-are-timeouts-handled-by-httpclient
            var response = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // download
            var donwloadStream = await response.Content.ReadAsStreamAsync();
            // unzip
            GZipStream decompressedStream = new GZipStream(donwloadStream, CompressionMode.Decompress);
            // read csv
            return ReadFromCsv(decompressedStream);
        }
        
        private IEnumerable<IpLocationDbSourceDto> ReadFromCsv(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ",",
            };
            var reader = new StreamReader(stream);
            var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<GeoMap>();
            return  csv.GetRecords<IpLocationDbSourceDto>();
        }
    }

}
