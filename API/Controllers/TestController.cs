using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpGet]
        public IActionResult GetTestValues()
        {
            return Ok();
        }
    }
}

