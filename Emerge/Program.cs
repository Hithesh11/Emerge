using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http; // Import the necessary namespace for session support

var builder = WebApplication.CreateBuilder(args);

// Add session support
builder.Services.AddSession(options =>
{
    // Configure session options here if needed
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Configure JWT authentication
app.UseAuthentication(); // Enable authentication

app.UseAuthorization(); // Enable authorization

// Enable session
app.UseSession();

app.UseEndpoints(endpoints =>
{
    // Direct route to the SPA page
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Authentication}/{action=Login}/{id?}");
});

app.Run();
