using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProfileProject.Data;
using ProfileProject.Data.Requirements;
using ProfileProject.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

builder.Services.AddScoped<BasicAuthControl>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<PostService>();

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

app.Run();
