using System.Text;
using API.Data;
using API.Interfaces;
using API.Middlwares;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("PGdbConnection") ?? throw new InvalidOperationException("Connection string 'PGdbConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); // auto-generated OpenAPI documentation

builder.Services.AddCors();
builder.Services.AddScoped<IMemberRepository,MemberRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(option => 
 {var tokenKey = builder.Configuration["TokenKey"]
    ?? throw new Exception("Token Key Not found in program.cs file");
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer =false,
        ValidateAudience = false
    };
 });
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

//app.UseHttpsRedirection();

//app.UseDeveloperExceptionPage(); // default exception page
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(s=>s.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope(); // if db not exist directly create the db structure and seed the mock data.
var services =scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}catch(Exception ex)
{
    var logger = services.GetRequiredService<Logger<Program>>();
 logger.LogError(ex,"An error occured during migration");
}

app.Run();
