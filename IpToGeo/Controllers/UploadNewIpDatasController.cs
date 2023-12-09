using Microsoft.AspNetCore.Mvc;
using IpToGeo.Services;



namespace IpToGeo.Controllers
{
    [Route("api/updater")]
    [ApiController]
    public class UploadNewIpDatasController : ControllerBase
    {

        private readonly IIpGeoService _ipGeoService;
        public UploadNewIpDatasController(IIpGeoService updateIpGeo) => _ipGeoService = updateIpGeo;

        [HttpPost]
        public async Task<IActionResult> TriggerIpGeoDatabaseUpdate()
        {
            try
            {
                await _ipGeoService.UpdateIpGeoDataAsync();
                return NoContent();
            }
            catch
            {
                return UnprocessableEntity();
            }
        }
    }
}

