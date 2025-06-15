using Aplikacija.Data;
using Aplikacija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AuthenticationDisplayName = null;
    options.AutomaticAuthentication = false;
});

builder.Services.Configure<IISOptions>(options =>
{
    options.ForwardClientCertificate = false;
    options.AutomaticAuthentication = false;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();



builder.Services.AddDefaultIdentity<Korisnik>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.Use(async (context, next) =>
{
    // AGRESIVNO UKLONI WINDOWS AUTH
    if (context.User.Identity.IsAuthenticated &&
        (context.User.Identity.AuthenticationType == "Negotiate" ||
         context.User.Identity.AuthenticationType == "NTLM" ||
         context.User.Identity.Name?.Contains("\\") == true))
    {
        context.User = new System.Security.Claims.ClaimsPrincipal();
    }
    await next();
});
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
