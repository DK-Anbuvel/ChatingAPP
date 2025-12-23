using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Data; // Assuming AppDbContext is in the API.Data namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

 [Route("api/[controller]")]
 [ApiController]
public class BaseAPIController :ControllerBase
{

}
