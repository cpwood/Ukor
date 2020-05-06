using Microsoft.AspNetCore.Mvc;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InstallController : ControllerBase
    {
        [HttpPost]
        [Route("{id}")]
        public IActionResult Install(string id)
        {
            return Ok();
        }
    }
}