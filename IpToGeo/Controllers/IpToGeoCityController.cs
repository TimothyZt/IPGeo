using IpToGeo.Services;
using Microsoft.AspNetCore.Mvc;
namespace IpToGeo.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpToGeoCityController : ControllerBase
    {
        private readonly IIpGeoService _ipGeoService;
        public IpToGeoCityController(IIpGeoService updateIpGeo)
        {
            _ipGeoService = updateIpGeo;
        }

        [HttpGet("{anyIp}")]
        public async Task<IActionResult> GetAnyIp(string anyIp)
        {
            try
            {
                var result = await _ipGeoService.GetIpv4GeoInfoAsync(anyIp);
                return Ok(result);
            }
            catch
            {
                return UnprocessableEntity();
            }
        }
    }
}
