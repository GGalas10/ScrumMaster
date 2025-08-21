using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScrumMaster.Sprints.Infrastructure;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var isTesting = builder.Environment.EnvironmentName == "Testing";
// Add services to the container.
if (!isTesting)
    builder.Services.AddDbContext<SprintDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConection")));
builder.Services.AddControllersWithViews();



var jwtSettings = builder.Configuration.GetSection("JwtSettings");
SymmetricSecurityKey key;
if (isTesting)
    key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("s+Qr8+VhSWEHHuwyqwP0kNvtg3HCSEX25A3MP1iENH4="));
else
    key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = false,
            ValidateIssuer = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["AccessToken"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ScrumMaster Sprints API",
        Version = "v1",
        Description = "API do zarz�dzania sprintami w ScrumMaster."
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Podaj token JWT (Bearer {token})",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization();
builder.Services.AddInfratstructure();
var front = builder.Configuration["Front:URL"];
builder.Services.AddCors(options => options.AddPolicy("AllowFrontend", policy =>
{
    policy.WithOrigins(front!)
          .AllowAnyMethod()
          .AllowCredentials()
          .WithHeaders("ScrumMaster", "Content-Type", "Authorization");
}));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScrumMaster Sprints API v1");
    c.RoutePrefix = string.Empty;
});
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRouting();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScrumMaster Tasks API v1");
    c.RoutePrefix = string.Empty;
});
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
public partial class Program{}