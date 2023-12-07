using CsvHelper.Configuration;
using CsvHelper;
using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;
using System.IO;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using System.Timers;

using NCrontab;
using IpToGeo.MyServices;



namespace IpToGeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadNewIpDatasController : ControllerBase
    {

        private readonly MyDbContext _myDbContext;
        private readonly UpdateIpGeoService _updateIpGeoService;
    
        private  System.Timers.Timer _Timer;
        public UploadNewIpDatasController(MyDbContext context,UpdateIpGeoService updateIpGeo)
        {
            _myDbContext = context;
            _updateIpGeoService = updateIpGeo;
          
           
        }
     


        #region 系统自动更新 
        //https://marcus116.blogspot.com/2018/02/c-web-api-httpclient.html
        [HttpPost]
        public async Task<bool> SystemAutoUploadFile()
        {
            string fileName = "geolite2-city-ipv4.csv.gz";
            string directorName = @".\";
            string fileFullPath = @".\geolite2-city-ipv4.csv";
            string downloadPath = "https://raw.githubusercontent.com/sapics/ip-location-db/main/geolite2-city/geolite2-city-ipv4.csv.gz";


            //DownloadGitData(fileName, DownloadPath);
            //GzUnzip(filePath);

          bool s =  await _updateIpGeoService.UpdateGo(fileName, downloadPath, directorName, fileFullPath);
           if(s)return true;
           return false;
        }
        #endregion

   
    }
}

