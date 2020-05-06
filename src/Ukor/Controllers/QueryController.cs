using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ukor.Configuration;
using Ukor.StaticResponses;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/xml")]
    public class QueryController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<ApplicationOptions> _options;

        public QueryController(
            IWebHostEnvironment env,
            IOptions<ApplicationOptions> options)
        {
            _env = env;
            _options = options;
        }

        [HttpGet]
        [Route("apps")]
        public ApplicationList GetApplications()
        {
            return _options.Value.List;
        }

        [HttpGet]
        [Route("active-app")]
        public QueryActiveAppResponse GetActiveApp()
        {
            return new QueryActiveAppResponse();
        }

        [HttpGet]
        [Route("media-player")]
        public QueryMediaPlayerResponse GetMediaPlayer()
        {
            return new QueryMediaPlayerResponse();
        }

        [HttpGet]
        [Route("device-info")]
        public QueryDeviceInfoResponse GetDeviceInfo()
        {
            return new QueryDeviceInfoResponse();
        }

        [HttpGet]
        [Route("icon/{id}")]
        public IActionResult GetIcon(int id)
        {
            var bytes = System.IO.File.ReadAllBytes(_env.WebRootFileProvider.GetFileInfo("app.jpg")?.PhysicalPath);
            return File(bytes, "image/jpeg");
        }
    }
}