using MangoTreesAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MangoTreesAPI.Models.ResponseMessages;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet("validate Token")]
        public string TokenIsWorking()
        {
            return "Token is working";
        }

        [Authorize(Roles = "customer")]
        [HttpGet("validate Token customer")]
        public string TokenIsWorking2()
        {
            return ResponseMessages.Response.Success.ToString();
        }
    }
}
