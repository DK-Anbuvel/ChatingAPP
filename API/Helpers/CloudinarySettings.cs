using System;
using API.Entities;

namespace API.Helpers;

public class CloudinarySettings
{
    public required string cloudName {get;set;}
    public required string ApiKey {get;set;}
    public required string ApiSecret {get;set;}
}