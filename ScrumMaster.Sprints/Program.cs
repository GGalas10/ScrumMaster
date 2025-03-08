using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure;
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
            ValidateIssuer = !isTesting
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddInfratstructure();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
public partial class Program { }