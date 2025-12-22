using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController(AppContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult<List<AppUser>> GetMembers()
        {
            //context.
            return Ok(new[] { "Member1", "Member2", "Member3" });
        }
    }
}
