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
        private readonly MyDbContext _myDbContext;
        public  UpdateIpGeoService(MyDbContext context)
        {
            _myDbContext = context;
          
        }

        public async Task<bool> UpdateGo(string fileName, string downloadPath,string directorName,string fileFullPathNotExtan) 
        {
            DownloadGitData(fileName, downloadPath);
            GzUnzip(directorName);
          
            return true;

        }

        #region 请求下载
        protected bool DownloadGitData(string fileName, string path)
        {

            // Seting up the http client used to download the data
            using (var client = new HttpClient())
            {
                //设置header信息
                client.Timeout = TimeSpan.FromMinutes(3);

                // Create a file stream to store the downloaded data.
                // This really can be any type of writeable stream.

                //https://stackoverflow.com/questions/20661652/progress-bar-with-httpclient
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
        //https://learn.microsoft.com/zh-tw/dotnet/standard/io/how-to-compress-and-extract-files
        protected async Task<bool> GzUnzip(string filePath)
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


    }
}
