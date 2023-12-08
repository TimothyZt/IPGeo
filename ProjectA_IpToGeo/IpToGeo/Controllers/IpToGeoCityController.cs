using IpToGeo.IpToCityDbContext;
using IpToGeo.Models;
using IpToGeo.MyServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IpToGeo.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpToGeoCityController : ControllerBase
    {
        private readonly FindIpToGeoService _findIpToGeoService;
        public IpToGeoCityController(FindIpToGeoService findIpToGeoService)
        {
            _findIpToGeoService = findIpToGeoService;
        }

        [HttpGet("{anyIp}")]
        public GeoliteCityIpv4String GetAnyIp(string anyIp)
        {
            var result = _findIpToGeoService.GetAnyIp(anyIp);
            return result;
        }

        [HttpGet]
        public ulong GetDe_Ip(string ip)
        {
            var s = _findIpToGeoService.IP_To_Num(ip);
            return s;
        }



    }
}
