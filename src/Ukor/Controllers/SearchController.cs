using Microsoft.AspNetCore.Mvc;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpPost]
        [Route("browse")]
        public IActionResult Browse()
        {
            return Ok();
        }
    }
}