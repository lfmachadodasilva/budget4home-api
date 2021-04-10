using budget4home.App.Expenses;
using budget4home.App.Groups;
using budget4home.App.Labels;
using budget4home.App.Users;
using budget4home.Util;
using Microsoft.Extensions.DependencyInjection;

namespace budget4home.Extensions
{
    public static class DependecyInjectionExtension
    {
        public static IServiceCollection SetupDependecyInjection(this IServiceCollection services)
        {
            // general
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICache, Cache>();

            // label
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<ILabelRepository, LabelRepository>();

            // expense
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            // group
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();

            // user
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFirebaseRepository, FirebaseRepository>();

            return services;
        }
    }
}