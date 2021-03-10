using budget4home.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home.Extensions
{
    public static class ContextExtension
    {
        public static IServiceCollection SetupContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringName = configuration.GetSection("AppConfig:ConnectionString").Value;
            if (connectionStringName == "Local")
            {
                services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("budget4home"));
            }
            else
            {
                services.AddDbContext<Context>(
                    opt => opt.UseNpgsql(
                        configuration.GetConnectionString(connectionStringName),
                        builder => builder.MigrationsAssembly("budget4home")));
            }

            return services;
        }
    }
}