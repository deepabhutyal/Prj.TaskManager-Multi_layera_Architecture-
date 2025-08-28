using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prj.TaskManager;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using Prj.TaskManager.Sevice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Identity register

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    options.SignIn.RequireConfirmedPhoneNumber = false;
//    options.SignIn.RequireConfirmedEmail = false;
//    options.SignIn.RequireConfirmedAccount = false;
//}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/NoPermission";
    });

builder.Services.AddControllersWithViews();

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
