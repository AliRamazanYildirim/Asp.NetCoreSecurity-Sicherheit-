using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ProdukteAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AdventureWorks2019Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region AddDefaultPolicy
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyOrigin()
//    .AllowAnyHeader()
//    .AllowAnyMethod();
//    });
//});
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7220")
       .AllowAnyHeader()
       .AllowAnyMethod();
    });

    options.AddPolicy("AllowPolicy2", policy =>
    {
        policy.WithOrigins("https://localhost:7220")
       .WithHeaders(HeaderNames.ContentType, "meine-header")
       .AllowAnyMethod();
    });

    options.AddPolicy("AllowPolicy3", policy =>
    {
        policy.WithOrigins("https://*.example.com:7220").SetIsOriginAllowedToAllowWildcardSubdomains()
       .AllowAnyHeader()
       .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseRouting();

//app.UseCors();

app.UseCors("AllowPolicy");

app.UseCors("AllowPolicy2");

app.UseCors("AllowPolicy3");

app.UseAuthorization();

app.MapControllers();

app.Run();
