using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Ukor.Configuration;
using Ukor.Services;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LaunchController : ControllerBase
    {
        private readonly IApplicationService _service;
        private readonly IOptions<ApplicationOptions> _options;

        public LaunchController(
            IApplicationService service,
            IOptions<ApplicationOptions> options)
        {
            _service = service;
            _options = options;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> LaunchAppAsync(string id)
        {
            if (int.TryParse(id, out var appId))
            {
                var app = _options.Value.Applications.FirstOrDefault(x => x.Id == appId);

                if (app == null)
                    return NotFound();

                await _service.DoActionAsync(app);

                return Ok();
            }

            return Forbid();
        }
    }
}