using System.Text;
using hangnow_back.Authentications;
using hangnow_back.Manager;
using hangnow_back.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {NamingStrategy = new CamelCaseNamingStrategy()};
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    }
);

builder.Services.AddTransient<EventManager>();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("App") ?? string.Empty));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => { })
    .AddEntityFrameworkStores<Context>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings")["Secret"] ??
                                          throw new InvalidOperationException());

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy(Roles.User, policy => policy.RequireRole(Roles.User));
    options.AddPolicy(Roles.PremiumUser, policy => policy.RequireRole(Roles.PremiumUser));
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSignalR();

// Build the application and return it

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedDataApplicationRoles.SeedRoles(services.GetRequiredService<RoleManager<IdentityRole<Guid>>>());
}

if (builder.Environment.IsDevelopment())
{
    app.UseCors(corsSettings => corsSettings
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    app.UseSwagger();
    app.UseSwaggerUI();
} 
else 
{
    app.UseCors(corsSettings => corsSettings.WithOrigins("https://mon-front.com"));
}

// custom jwt auth middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();