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
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            {
                services.AddTransient<ILabelService, LabelService>();
                services.AddTransient<ILabelRepository, LabelRepository>();
            }
            {
                services.AddTransient<IExpenseService, ExpenseService>();
                services.AddTransient<IExpenseRepository, ExpenseRepository>();
            }
            {
                services.AddTransient<IGroupService, GroupService>();
                services.AddTransient<IGroupRepository, GroupRepository>();
            }
            {
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IFirebaseRepository, FirebaseRepository>();
            }

            return services;
        }
    }
}