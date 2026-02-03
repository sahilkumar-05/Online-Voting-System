using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Add services
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<OnlineVSDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor(); // Inject HttpContext in Razor

var app = builder.Build();

// Use session middleware
app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();
app.UseSession();  // Must be before UseAuthorization

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
