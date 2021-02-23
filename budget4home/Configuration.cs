using budget4home.Models;
using budget4home.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home
{
    public static class Configuration
    {
        public static IApplicationBuilder DatabaseMigrate(this IApplicationBuilder app, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("AppConfig:ConnectionString").Value;
            if (connectionString != "Local")
            {
                using (var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<Context>())
                    {
                        // context.Database.EnsureDeleted();
                        context.Database.Migrate();

                        // Seed.Run(context);
                    }
                }
            }

            return app;
        }
    }
}