using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseAPIController
{

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        if (await EmailExists(dto.Email)) return BadRequest("Email Is Exists");

        using var hmac = new HMACSHA512(); //using for dispose the memeory

        var user = new AppUser
        {
            DisplayName = dto.DisplayName,
            Email = dto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await context.Users
                .SingleOrDefaultAsync(s => s.Email == dto.Email);
        if (user == null) return Unauthorized("Invalid Email");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        for (var i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        return  user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string Email)
    {
        return await context.Users.AnyAsync(s => s.Email.ToLower() == Email.ToLower());
    }
}
