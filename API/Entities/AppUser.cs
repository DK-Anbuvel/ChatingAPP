using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Primitives;

namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid()
        .ToString();// Generates a new unique identifier for each user when an instance is created
    public required string DisplayName { get; set;}
    public required string Email { get; set;}
    public  string? Imagurl { get; set;} //duplicate propery for optimacy
    public required byte[]? PasswordHash { get; set; }
    public required byte[]? PasswordSalt { get; set; }

// 

public Member Member {get;set;}=null!;

}

