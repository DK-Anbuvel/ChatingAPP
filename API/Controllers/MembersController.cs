using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Data; // Assuming AppDbContext is in the API.Data namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

   [Authorize]
    public class MembersController(AppDbContext context) : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return Ok(members);
        }  
        [AllowAnonymous] 
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMembers(string id)
        {
            var members = await context.Users.FindAsync(id);
            if (members == null) return NotFound();
            return Ok(members);
        }   
    }
}
