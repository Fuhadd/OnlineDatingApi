
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineDatingApi;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Core.Services.Authentication;
using OnlineDatingApi.Core.Services.RefreshTokenServices;
using OnlineDatingApi.Core.Services.UserClaims;
using OnlineDatingApi.Data.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Environment.GetEnvironmentVariable("KEY");
var tokenValidationParams = new TokenValidationParameters
{
    ClockSkew = TimeSpan.Zero,

    ValidateIssuer = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false,

    ValidIssuer = jwtSettings.GetSection("Issuer").Value,

    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
};

// Add services to the container.
builder.Services.AddTransient<IUnitofWork, UnitofWork>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IRefreshToken, RefreshTokenServices>();
builder.Services.AddScoped<IUserClaims, UserClaims>();
builder.Services.ConfigureJwt(tokenValidationParams);
builder.Services.AddSingleton(tokenValidationParams);
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureAutoMapper();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityOptions();
builder.Services.ConfigureSwagger();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers().AddNewtonsoftJson(options => 
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Global Exception Handling, Uncomment Below
//app.ConfigureExceptionHandler();

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
