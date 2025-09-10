using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using AuthenticationTest.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Required for using IdentityRole
builder.Services.AddRazorPages();
//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Claim settings
    options.ClaimsIdentity.EmailClaimType = "Email";
    options.ClaimsIdentity.RoleClaimType = "UserRole";
    options.ClaimsIdentity.SecurityStampClaimType = "SecurityStamp";
    options.ClaimsIdentity.UserIdClaimType = "UserID";
    options.ClaimsIdentity.UserNameClaimType = "UserName";

    // Lockout settings.
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // Password options
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6; // Set minimum length
    options.Password.RequiredUniqueChars = 1; // Set minimum unique chars
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    // SignIn options
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Store options
    options.Stores.MaxLengthForKeys = 128;
    options.Stores.ProtectPersonalData = false;
    options.Stores.SchemaVersion = new Version("1.0.0.0");

    // Token options
    options.Tokens.AuthenticatorIssuer = "MyApp";
    options.Tokens.AuthenticatorTokenProvider = "Default";
    options.Tokens.ChangeEmailTokenProvider = "Default";
    options.Tokens.ChangePhoneNumberTokenProvider = "Default";
    options.Tokens.EmailConfirmationTokenProvider = "Default";
    options.Tokens.PasswordResetTokenProvider = "Default";
    options.Tokens.ProviderMap["Default"] = new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<IdentityUser>));

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie((CookieAuthenticationOptions options) =>
    {
        options.AccessDeniedPath = "/Home/AccessDenied"; //"/Identity/Account/AccessDenied"; // Path for unauthorized users
        options.LoginPath = "/Home/Login"; //"/Identity/Account/Login"; // Path for unauthenticated users
        options.LogoutPath = "/Home/Logout"; //"/Identity/Account/Logout";
        options.ReturnUrlParameter = "MACARONI";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = new TimeSpan(0, 10, 0);
        //options.SessionStore = new SessionS
        
        // You can also set other options like SlidingExpiration, ExpireTimeSpan, etc.
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Login"; // Example login path
    options.AccessDeniedPath = "/Home/AccessDenied"; // Correct path to your Access Denied page
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseAuthentication(); // Must be before UseAuthorization()
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
