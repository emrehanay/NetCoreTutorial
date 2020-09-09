using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreTutorial.Helpers;
using NetCoreTutorial.Model;
using NetCoreTutorial.Service;

namespace NetCoreTutorial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("Posts")]
        public async Task<IActionResult> Posts()
        {
            var result = await _service.GetPosts();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] PostEditRequest request)
        {
            var appUser = (AppUser) HttpContext.Items["User"];
            var result = await _service.AddPost(appUser, request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("EditPost")]
        public async Task<IActionResult> EditPost([FromBody] PostEditRequest request)
        {
            var appUser = (AppUser) HttpContext.Items["User"];
            var result = await _service.EditPost(appUser, request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            var appUser = (AppUser) HttpContext.Items["User"];
            await _service.DeletePost(appUser, id);
            return Ok();
        }
    }
}