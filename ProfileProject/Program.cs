using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProfileProject.Data;
using ProfileProject.Data.Requirements;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? @"Server=(localdb)\\mssqllocaldb;Database=ProfileProject_Local_DB;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //These are turned off for DEMO purposes
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.MaxFailedAccessAttempts = 8;
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/profile/signin/";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAllowedToEdit", policy =>
    policy.Requirements.Add(new ProfileOwnerRequirement()));
    options.AddPolicy("IsPostOwner", polity =>
    polity.Requirements.Add(new PostOwnerRequirement()));
});

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthControl, BasicAuthControl>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IDAL, DAL>();
builder.Services.AddScoped<IImageProcessor, ImageProcessor>();
//To delete, NOTE TO SELF: Do NOT request, USE interface instead
//builder.Services.AddScoped<BasicAuthControl>();
//builder.Services.AddScoped<ProfileService>();

builder.Services.AddScoped<IAuthorizationHandler, ProfileOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PostOwnerAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("/Home/Error");
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "area",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action}/{username?}");
app.MapRazorPages();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
