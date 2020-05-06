using Microsoft.AspNetCore.Mvc;
using Rssdp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Ukor.Configuration;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        private readonly IOptions<LocalServerOptions> _localOptions;

        public DiscoveryController(IOptions<LocalServerOptions> localOptions)
        {
            _localOptions = localOptions;
        }

        [HttpGet]
        public async Task<IEnumerable<DiscoveredSsdpDevice>> ListAsync([FromQuery]string filter)
        {
            using var deviceLocator = new SsdpDeviceLocator(_localOptions.Value.IpAddress);
            return await deviceLocator.SearchAsync(filter ?? "roku:ecp");
        }
    }
}