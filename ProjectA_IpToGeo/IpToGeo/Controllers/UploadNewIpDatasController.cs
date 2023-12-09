﻿using CsvHelper.Configuration;
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

        private readonly UpdateIpGeoService _updateIpGeoService;
        public UploadNewIpDatasController(UpdateIpGeoService updateIpGeo)
        {
            _updateIpGeoService = updateIpGeo;
        }

        /// <summary>
        /// 请求更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SystemAutoUploadFile()
        {
            bool s = await _updateIpGeoService.UpdateGo();
            return s;
            
        }
    


    }
}

