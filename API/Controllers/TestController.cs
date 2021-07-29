using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpGet]
        public IActionResult GetTestValues()
        {
            return Ok("You are welcome");
        }
    }
}

