using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home.Extensions
{
    public static class CacheExtension
    {
        public static IServiceCollection SetupCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = configuration.GetConnectionString("Redis");
            });

            return services;
        }
    }
}
