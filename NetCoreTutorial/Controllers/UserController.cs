using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreTutorial.Model;
using NetCoreTutorial.Service;

namespace NetCoreTutorial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _service.Login(request);
            return Ok(result);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var result = await _service.SignUp(request);
            return Ok(result);
        }
    }
}