using Microsoft.EntityFrameworkCore;
using MoreMusic.DataLayer;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;
using MoreMusic.DataLayer.Entity;

var builder = WebApplication.CreateBuilder(args);

// Build the configuration using appsettings.json and other configuration sources
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure PostgreSQL connection
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MusicDbContext>()
    .AddDefaultTokenProviders();

builder.Logging.AddConsole();

string audioFilesBasePath = builder.Configuration.GetSection("AudioFilesBasePath").Value;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Serve static files from the path specified in the appsettings with the URL path "/MusicFiles/"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(audioFilesBasePath),
    RequestPath = "/MusicFiles",
    ServeUnknownFileTypes = true, // This allows serving unknown file types like MP3
    DefaultContentType = "audio/aac" // Set the content type for audio files
});

app.Run();
