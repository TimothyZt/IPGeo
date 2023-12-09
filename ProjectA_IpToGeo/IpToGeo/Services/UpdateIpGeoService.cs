using CsvHelper.Configuration;
using CsvHelper;
using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using System.Globalization;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;

namespace IpToGeo.MyServices
{
    public class UpdateIpGeoService
    {
        private readonly string fileName = "geolite2-city-ipv4.csv.gz";
        private readonly string directorName = @".\";
        private readonly string fileFullPathNotExtan = @".\geolite2-city-ipv4.csv";
        private readonly string downloadPath = "https://raw.githubusercontent.com/sapics/ip-location-db/main/geolite2-city/geolite2-city-ipv4.csv.gz";
        private readonly IpToGeoDbContext _myDbContext;

        public UpdateIpGeoService(IpToGeoDbContext context)
        {
            _myDbContext = context;
        }

        public async Task<bool> UpdateGo()
        {
            await Download(fileName, downloadPath);
            await GzUnzip(directorName);
            CreateTable();
            DeleteTable();
            AlterTableName();
            await NoheadUploadSmallFile(fileFullPathNotExtan);
            return true;
        }

       /// <summary>
       /// 请求下载
       /// </summary>
       /// <param name="fileName"></param>
       /// <param name="path"></param>
       /// <returns></returns>
        protected async Task Download(string fileName, string path)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(3);
                using (var s = await client.GetStreamAsync(path))
                {
                    using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        s.CopyTo(fs);
                    }
                }
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected async Task GzUnzip(string filePath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

            foreach (FileInfo fileToDecompress in directoryInfo.GetFiles("*.gz"))
            {
                using (FileStream fileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);
                    using (FileStream decompressionStream = System.IO.File.Create(newFileName))
                    {
                        using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                        {
                            await gZipStream.CopyToAsync(decompressionStream);
                            Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected async Task NoheadUploadSmallFile(string filePath)
        {
            IEnumerable<GeoliteCityIpv4String> geoliteCityIpv4s_String;
            IEnumerable<GeoliteCityIpv4Int> geoliteCityIpv4s_Int;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ",",
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<GeoMap>();
                var data = csv.GetRecords<GeoliteCityIpv4String>().Select(a => new GeoliteCityIpv4Int()
                {
                    IpRangeStart = IP_To_Num(a.IpRangeStart),
                    IpRangeEnd = IP_To_Num(a.IpRangeEnd),
                    CountryCode = a.CountryCode,
                    State1 = a.State1,
                    State2 = a.State2,
                    City = a.City,
                    Postcode = a.Postcode,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Timezone = a.Timezone,
                });
                await _myDbContext.BulkInsertAsync(data);
            }
        }

        /// <summary>
        /// Ip变十进制整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        protected uint IP_To_Num(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        protected void CreateTable()
        {
            _myDbContext.Database.ExecuteSqlRaw(
                "CREATE TABLE `ipToGeoCity2`  (\n" +
                "`IpRangeStart` int UNSIGNED NOT NULL,\n" +
                "`IpRangeEnd` int UNSIGNED NOT NULL,\n" +
                "`CountryCode` text  ,\n" +
                " `State1` text  ,\n" +
                "`State2` text  ,\n" +
                "`City` text  ,\n" +
                "`Postcode` text  ,\n" +
                "`Latitude` text  ,\n" +
                "`Longitude` text ,\n" +
                "`Timezone` text  ,\n" +
                "INDEX ip_search (IpRangeStart DESC)\n" +
                ") ");
        }

        protected void AlterTableName() 
        {
            _myDbContext.Database.ExecuteSqlRaw(
                "ALTER TABLE ipToGeoCity2 RENAME TO ipToGeoCity \n"
                ) ;
        }



        /// <summary>
        /// 删除表
        /// </summary>
        protected void DeleteTable()
        {
            _myDbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS `ipToGeoCity`;");
            _myDbContext.SaveChanges();
        }
       
    }
}
