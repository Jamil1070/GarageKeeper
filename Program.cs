using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Data;
using Microsoft.AspNetCore.Identity;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Areas.Identity.Data;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Configure the database context
builder.Services.AddDbContext<GarageContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GarageContext") ?? throw new InvalidOperationException("Connection string 'Garage2Context' not found.")));

// Configure identity services with roles
builder.Services.AddDefaultIdentity<GarageUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GarageContext>();

// Register custom middleware
builder.Services.AddTransient<CookieMiddleware>();

// Configure localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Add controllers with views support
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Set up supported cultures for localization
var supportedCultures = new[] { "en-US", "fr", "nl" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Initialize the database with required data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GarageContext>();
    Initializer.DbSetInitializer(context);
}

// Configure error handling for non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Enable serving static files
app.UseStaticFiles();

// Configure the routing system
app.UseRouting();

// Enable authorization middleware
app.UseAuthorization();

// Map Razor pages (for identity)
app.MapRazorPages();

// Use the custom cookie middleware
app.UseMiddleware<CookieMiddleware>();

// Configure the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed roles into the database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Seed initial users (admin and normal user) into the database
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<GarageUser>>();

    // Create admin user if not exists
    string email = "admin@admin.com";
    string password = "Admin123!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GarageUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            ConfirmEmail = email,
            Adress = "Adminstreet 1",
            DateOfBirth = DateTime.Now.AddYears(-100),
            Name = "Admin",
            Surname = "Admin",
            IsAdmin = true
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }

    // Create normal user if not exists
    string userEmail = "user@user.com";
    string userPassword = "User123!";

    if (await userManager.FindByEmailAsync(userEmail) == null)
    {
        var normalUser = new GarageUser
        {
            UserName = userEmail,
            Email = userEmail,
            EmailConfirmed = true,
            ConfirmEmail = userEmail,
            Adress = "Userstreet 1",
            DateOfBirth = DateTime.Now.AddYears(-25),
            Name = "User",
            Surname = "User",
            IsAdmin = false
        };

        var userResult = await userManager.CreateAsync(normalUser, userPassword);

        if (userResult.Succeeded)
        {
            await userManager.AddToRoleAsync(normalUser, "User");
        }
    }
}

// Run the application
app.Run();
