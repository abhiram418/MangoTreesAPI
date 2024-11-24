using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangoTreesAPI.Middleware
{
    public static class AuthenticationMiddleware
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")?? configuration["JwtSettings:SecretKey"]?? "";

            services.Configure<AuthenticationOptions>(options =>
            {
                options.SecretKey = SecretKey;
            });

            if (string.IsNullOrEmpty(SecretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured");
            }

            var SecretKey_Bytes = Encoding.UTF8.GetBytes(SecretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey_Bytes),
                    ValidateLifetime = true,
                    RoleClaimType = "role_in_mangotrees",
                    NameClaimType = ClaimTypes.Name,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();
                        var SessionId = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                        var SessionValidity = await authService.IsSessionValidAsync(SessionId!);
                        if (SessionId == null || !SessionValidity)
                        {
                            context.Fail("Session is revoked");
                        }
                    }
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureSwaggerAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
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

            return services;
        }
    }
}
