using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ukor.Configuration;
using Ukor.Services;

namespace Ukor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KeyPressController : ControllerBase
    {
        private readonly IKeyPressService _service;

        public KeyPressController(
            IKeyPressService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> DoKeyPress(Application.KeyPress id)
        {
            await _service.HandleKeyPress(id);
            return Ok();
        }
    }
}