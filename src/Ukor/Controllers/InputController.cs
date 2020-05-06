using Microsoft.AspNetCore.Mvc;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InputController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveInput()
        {
            return Ok();
        }
    }
}