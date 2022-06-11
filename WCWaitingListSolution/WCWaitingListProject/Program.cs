using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WCWaitingListProject.Models;
using WCWaitingListSolution.Data;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<WCWaitingListProjectContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("WCWaitingListProjectContext")));
    builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<WCWaitingListProjectContext>();builder.Services.AddDbContext<WCWaitingListProjectContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WCWaitingListProjectContext")));
    builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");
}
else
{
    builder.Services.AddDbContext<WCWaitingListProjectContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionWCWaitingListProjectContext")));
    builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");
    // .AddEntityFrameworkStores<WCWaitingListProjectContext>();builder.Services.AddDbContext<WCWaitingListProjectContext>(options =>
    // options.UseSqlServer(builder.Configuration.GetConnectionString("WCWaitingListProjectContext")));
}
// builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<WCWaitingListProjectContext>();builder.Services.AddDbContext<WCWaitingListProjectContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("WCWaitingListProjectContext")));

// builder.Services.AddIdentity<AppUser, IdentityRole>()
//     .AddEntityFrameworkStores<WCWaitingListProjectContext>()
//     ;

builder.Services.AddAuthorization(options => 
    {
        options.AddPolicy("adminAndHigherPolicy", policy => 
            policy.RequireAssertion(context => 
                {
                    return context.User.Claims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value.Equals("Admin"));
                }));
        options.AddPolicy("tutorAndHigherPolicy", policy => 
            policy.RequireAssertion(context => 
                {
                    return context.User.Claims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value.Equals("Admin")) ||
                        context.User.Claims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value.Equals("Tutor"));
                }));
    }
);

//builder.Services.AddScoped<IUserClaimsPrincipalFactory <AppUser>, MyUserClaimsPrincipleFactory>

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapDefaultControllerRoute();
    });
app.Run();
