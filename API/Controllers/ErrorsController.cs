using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("[controller]/{code}")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {


        public ActionResult Error(int code)
        {
            return new ObjectResult(new APIResponse(code));
        }
    }
}
