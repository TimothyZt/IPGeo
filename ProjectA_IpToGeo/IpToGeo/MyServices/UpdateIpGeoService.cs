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

        public  UpdateIpGeoService(IpToGeoDbContext context)
        {
            _myDbContext = context;    
        }

        public async Task<bool> UpdateGo() 
        {
            DownloadGitData(fileName, downloadPath);
            GzUnzip(directorName);
            DeleteTable();
            CreateTable();
           await  NoheadUploadSmallFile(fileFullPathNotExtan);
            return true;

        }

        #region 请求下载
        protected bool DownloadGitData(string fileName, string path)
        {

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(3); 
                using (var s = client.GetStreamAsync(path))
                {
                    using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        s.Result.CopyTo(fs);
                    }
                }
            }
            return true;
        }
        #endregion

        #region 解压gz压缩包
        protected  bool GzUnzip(string filePath)
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
                            gZipStream.CopyTo(decompressionStream);
                            Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region 无头更新插数据

        protected async Task<bool> NoheadUploadSmallFile(string filePath)
        {
            IEnumerable<GeoliteCityIpv4_String> geoliteCityIpv4s_String;
            IEnumerable<GeoliteCityIpv4_Int> geoliteCityIpv4s_Int;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ",",
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<GeoMap>();
                var data = csv.GetRecords<GeoliteCityIpv4_String>().Select(a => new GeoliteCityIpv4_Int()
                {
                    ip_range_start = IP_To_Num(a.ip_range_start),
                    ip_range_end = IP_To_Num(a.ip_range_end),
                    country_code = a.country_code,
                    state1 = a.state1,
                    state2 = a.state2,
                    city = a.city,
                    postcode = a.postcode,
                    latitude = a.latitude,
                    longitude = a.longitude,
                    timezone = a.timezone,
                });
            
                await _myDbContext.BulkInsertAsync(data); 
            }
            return true;
        }

        protected ulong IP_To_Num(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return ulong.Parse(items[0]) << 24
                    | ulong.Parse(items[1]) << 16
                    | ulong.Parse(items[2]) << 8
                    | ulong.Parse(items[3]);
        }
        #endregion


        #region 创建表 
        protected void CreateTable()
        {
            _myDbContext.Database.ExecuteSqlRaw(
                "CREATE TABLE `ipToGeoCity`  (\r\n  " +
                "`ip_range_start` bigint UNSIGNED NOT NULL,\r\n  " +
                "`ip_range_end` bigint UNSIGNED NOT NULL,\r\n  " +
                "`country_code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n " +
                " `state1` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`state2` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`city` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`postcode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`latitude` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`longitude` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n  " +
                "`timezone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,\r\n" +
                "INDEX ip_search (ip_range_start DESC,ip_range_end DESC)\r\n" +
                ") ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;");
        }
        #endregion

        #region 删除表
        protected void DeleteTable()
        {
            _myDbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS `ipToGeoCity`;");
            _myDbContext.SaveChanges();
        }
        #endregion
    }
}
