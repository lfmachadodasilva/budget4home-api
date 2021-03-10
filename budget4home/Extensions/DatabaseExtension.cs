using System.Linq;
using budget4home.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home.Extensions
{
    public static class DatabaseExtension
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
                        context.Database.Migrate();
                        if (!context.Groups.Any())
                        {
                            Seed.Run(context);
                        }
                    }
                }
            }

            return app;
        }
    }
}