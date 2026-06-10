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
using API.Services;

namespace API.Controllers
{

   [Authorize]
   [Route("api/[controller]")]
    public class MembersController(IMemberRepository memberRepo,
                 IPhotoService photoRepo) : BaseAPIController
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm]IFormFile File)
        {
            var member = await memberRepo.GetMebersByIdAsync(User.GetMemberId());

            if(member == null) return BadRequest("Cannot update member");

            var result = await photoRepo.UploadPhotoAsync(File);
            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };

            if(member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.Imagurl= photo.Url;
            }
            member.Photos.Add(photo);

            if(await memberRepo.SaveAllAsync()) return photo;
            return BadRequest("Problem adding photo");
        }  

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await memberRepo.GetMemberForUpdate(User.GetMemberId());
            if(member == null) return BadRequest("Cannot get member from token");

            var photo = member.Photos.SingleOrDefault(s=>s.Id == photoId);

            if(member.ImageUrl == photo?.Url || photo == null) return BadRequest("Cannot set this as main image");
            
            member.ImageUrl = photo.Url;
            member.User.Imagurl = photo.Url;

            if(await memberRepo.SaveAllAsync()) return NoContent();
            
            return BadRequest("Problem setting main photo");
        }
        
         [HttpGet("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {   
            var member = await memberRepo.GetMemberForUpdate(User.GetMemberId());
            if(member == null) return BadRequest("Cannot get member from token");

            var photo = member.Photos.SingleOrDefault(s=>s.Id == photoId);
             if(photo == null || photo.Url == member.ImageUrl)
             return BadRequest("This photo cannot be deleted");

             if(photo.PublicId != null)
            {
                var result = await photoRepo.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);

            }
            member.Photos.Remove(photo);
            if(await memberRepo.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting the photo");

        }
   }
}
