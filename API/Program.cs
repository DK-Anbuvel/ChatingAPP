using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("PGdbConnection") ?? throw new InvalidOperationException("Connection string 'PGdbConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); // auto-generated OpenAPI documentation

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
