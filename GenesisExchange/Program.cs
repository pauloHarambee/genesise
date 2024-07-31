using GenesisExchange.Data;
using GenesisExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Routing Behavior
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Dependence Injection
//var con = builder.Configuration
//.GetConnectionString("PRODConnectionString");
//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseMySql(con, ServerVersion.AutoDetect(con)));

var con = builder.Configuration
.GetConnectionString("AppConnectionString");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(con));

//Registering Identity Services
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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

app.Populate();
app.EnsureIdentity().Wait();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
