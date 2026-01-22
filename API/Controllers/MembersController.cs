using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Data; // Assuming AppDbContext is in the API.Data namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;

namespace API.Controllers
{

   [Authorize]
    public class MembersController(IMemberRepository memberRepo) : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetMembers()
        {
            var members = await memberRepo.GetMebersAsync();
            return Ok(members);
        }  
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMembers(string id)
        {
            var members = await memberRepo.GetMebersByIdAsync(id);
            if (members == null) return NotFound();
            return Ok(members);
        }   
       
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMembersPhotos(string id)
        {
            var members = await memberRepo.GetPhotosFormmberAsync(id);
            if (members == null) return NotFound();
            return Ok(members);
        }   
    }
}
