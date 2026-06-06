using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Repositories;
using CustomerEngagementPlatform.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // For this training project, email confirmation is disabled so login works directly after registration.
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddUserManager<CustomerEngagementPlatform.Helpers.CustomUserManager>()
.AddEntityFrameworkStores<ApplicationDbContext>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

// JWT is used for secure API endpoints.
// MVC pages will continue using Identity cookie login.
builder.Services.AddAuthentication()
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CustomerEngagementPlatform API", Version = "v1" });
    
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

var app = builder.Build();

// Seed Identity Roles and Staff User
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Seed Roles
    string[] roleNames = { "Customer", "Staff" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Seed Staff User
    var staffEmail = "staff@demo.com";
    var staffUser = await userManager.FindByEmailAsync(staffEmail);
    if (staffUser == null)
    {
        staffUser = new IdentityUser
        {
            UserName = staffEmail,
            Email = staffEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(staffUser, "Staff@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(staffUser, "Staff");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(staffUser, "Staff"))
        {
            await userManager.AddToRoleAsync(staffUser, "Staff");
        }
        if (await userManager.IsInRoleAsync(staffUser, "Customer"))
        {
            await userManager.RemoveFromRoleAsync(staffUser, "Customer");
        }
    }

    // Seed staff1@gmail.com if they exist
    var staff1Email = "staff1@gmail.com";
    var staff1User = await userManager.FindByEmailAsync(staff1Email);
    if (staff1User != null)
    {
        if (!await userManager.IsInRoleAsync(staff1User, "Staff"))
        {
            await userManager.AddToRoleAsync(staff1User, "Staff");
        }
        if (await userManager.IsInRoleAsync(staff1User, "Customer"))
        {
            await userManager.RemoveFromRoleAsync(staff1User, "Customer");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

app.MapRazorPages();

app.Run();