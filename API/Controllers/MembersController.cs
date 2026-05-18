using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Data; // Assuming AppDbContext is in the API.Data namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using System.Security.Claims;
using API.Extensions;

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

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemeberUpdateDto dto)
        {
           // var memeberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             var memeberId= User.GetMemberId();

            if(memeberId == null)  return BadRequest("Oops - no id found in token. ");

            var members = await memberRepo.GetMemberForUpdate(memeberId);
            if (members == null) return BadRequest("Could not get member");

            members.DisplayName = dto.DisplayName ?? members.DisplayName;
           members.Description = dto.Description ?? members.Description;
            members.City = dto.City ?? members.City;
            members.Country = dto.Country ?? members.Country;
            members.User.DisplayName = dto.DisplayName ?? members.User.DisplayName;
            
              memberRepo.Update(members);// optional
              
              if(await memberRepo.SaveAllAsync())
               return NoContent();
 
            return BadRequest("Failed to update member");
        }   
        
    }
}
