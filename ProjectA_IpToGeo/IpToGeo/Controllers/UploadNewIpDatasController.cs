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
    [Route("api/updater")]
    [ApiController]
    public class UploadNewIpDatasController : ControllerBase
    {

        private readonly IpGeoService _ipGeoService;
        public UploadNewIpDatasController(IpGeoService updateIpGeo)
        {
            _ipGeoService = updateIpGeo;
        }

        /// <summary>
        /// 请求更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TriggerIpGeoDatabseUpdate()
        {
            try
            {
                await _ipGeoService.UpdateGo();
                return NoContent();
            }
            catch 
            {
                return UnprocessableEntity();
            }
            
        }
    


    }
}

