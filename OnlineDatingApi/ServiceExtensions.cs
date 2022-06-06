using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineDatingApi.Data.Configurations;
using OnlineDatingApi.Data.Data;
using System.Text;

namespace OnlineDatingApi
{
    public static class ServiceExtensions
    {

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(options =>
            {
                options.User.RequireUniqueEmail = true;


            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);

            builder.AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
        }


        public static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            var builder = services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;


            });


        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperInitializer));
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Jwt Authorization header using the Bearer scheme. Enter 'Bearer' [space] 
                        and your token in the input text below.
                        Example: 'Bearer 1234....'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.
                AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "0auth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }

                });
            });
        }

        public static void ConfigureJwt(this IServiceCollection services,TokenValidationParameters tokenValidationParameters)
        {
            //var jwtSettings = configuration.GetSection("Jwt");
            //var key = Environment.GetEnvironmentVariable("KEY");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                //new TokenValidationParameters
                //{

                //    ValidateIssuer = false,
                //    ValidateLifetime = true,
                //    ValidateIssuerSigningKey = true,
                //    ValidateAudience = false,

                //    ValidIssuer = jwtSettings.GetSection("Issuer").Value,

                //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                //};
                
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please Try Again"
                        }.ToString());
                    }
                });
            });
        }
    }
}
