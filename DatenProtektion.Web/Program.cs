using DatenProtektion.Web.Filter;
using DatenProtektion.Web.IPMiddleware;
using DatenProtektion.Web.Modelle;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AdventureWorks2019Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<IpList>(builder.Configuration.GetSection("IpList"));
builder.Services.AddScoped<CheckWhiteList>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.UseMiddleware<IpListMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
