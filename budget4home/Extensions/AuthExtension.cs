using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace budget4home.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection SetupAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var firebaseAdmin = configuration.GetSection("AppConfig:FirebaseAdmin");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(firebaseAdmin.Value)
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://localhost:3000",
                                "https://localhost:3000",
                                "http://lfmachadodasilva.github.io",
                                "http://lfmachadodasilva.github.io/budget4home")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var projectId = configuration.GetSection("Firebase:ProjectId").Value;
                    options.Authority = $"https://securetoken.google.com/{projectId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{projectId}",
                        ValidateAudience = true,
                        ValidAudience = projectId,
                        ValidateLifetime = true
                    };
                });

            return services;
        }
    }
}