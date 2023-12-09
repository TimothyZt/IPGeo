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
        private readonly IpGeoService _ipGeoService;
        public IpToGeoCityController(IpGeoService updateIpGeo)
        {
            _ipGeoService = updateIpGeo;
        }

        [HttpGet("{anyIp}")]
        public GeoliteCityIpv4String GetAnyIp(string anyIp)
        {
            var result = _ipGeoService.GetAnyIp(anyIp);
            return result;
        }

    }
}
