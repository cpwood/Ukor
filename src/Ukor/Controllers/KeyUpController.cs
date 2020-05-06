using Microsoft.AspNetCore.Mvc;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KeyUpController : ControllerBase
    {
        [HttpPost]
        [Route("{id}")]
        public IActionResult DoKeyUp(string id)
        {
            return Ok();
        }
    }
}