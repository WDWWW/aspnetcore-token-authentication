using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Wd3w.TokenAuthentication.Sample.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : ControllerBase
    {

        private readonly ILogger<SampleController> _logger;

        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet("my-email")]
        public string GetMyEmail()
        {
            var email = User.Claims.First(c => c.Type == ClaimTypes.Email);
            return email.Value;
        }

        [HttpGet("public-api")]
        public string GetPublicApi()
        {
            return "Success!";
        }
    }
}