using Microsoft.AspNetCore.Mvc;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KeyDownController : ControllerBase
    {
        [HttpPost]
        [Route("{id}")]
        public IActionResult DoKeyDown(string id)
        {
            return Ok();
        }
    }
}