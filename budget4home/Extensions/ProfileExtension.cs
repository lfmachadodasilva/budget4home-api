using budget4home.App.Expenses;
using budget4home.App.Groups;
using budget4home.App.Labels;
using budget4home.App.Users;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home.Extensions
{
    public static class ProfileExtension
    {
        public static IServiceCollection SetupProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(LabelProfile));
            services.AddAutoMapper(typeof(ExpenseProfile));
            services.AddAutoMapper(typeof(GroupProfile));
            services.AddAutoMapper(typeof(UserProfile));

            return services;
        }
    }
}