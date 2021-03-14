using budget4home.App.Expenses;
using budget4home.App.Expenses.Validators;
using budget4home.App.Groups;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels;
using budget4home.App.Labels.Validators;
using budget4home.App.Users;
using budget4home.App.Users.Validators;
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
                services.AddTransient<ILabelValidator, LabelValidator>();
                services.AddTransient<IAddLabelValidator, AddLabelValidator>();
                services.AddTransient<IDeleteLabelValidator, DeleteLabelValidator>();
                services.AddTransient<IGetFullLabelsValidator, GetFullLabelsValidator>();
                services.AddTransient<IUpdateLabelValidator, UpdateLabelValidator>();
            }
            {
                services.AddTransient<IExpenseService, ExpenseService>();
                services.AddTransient<IExpenseRepository, ExpenseRepository>();
                services.AddTransient<IExpenseValidator, ExpenseValidator>();
                services.AddTransient<IAddExpenseValidator, AddExpenseValidator>();
                services.AddTransient<IDeleteExpenseValidator, DeleteExpenseValidator>();
                services.AddTransient<IGetExpensesValidator, GetExpensesValidator>();
                services.AddTransient<IUpdateExpenseValidator, UpdateExpenseValidator>();
            }
            {
                services.AddTransient<IGroupService, GroupService>();
                services.AddTransient<IGroupRepository, GroupRepository>();
                services.AddTransient<IGroupValidator, GroupValidator>();
                services.AddTransient<IGetByIdValidator, GetByIdValidator>();
                services.AddTransient<IAddGroupValidator, AddGroupValidator>();
                services.AddTransient<IDeleteGroupValidator, DeleteGroupValidator>();
                services.AddTransient<IUpdateGroupValidator, UpdateGroupValidator>();
            }
            {
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IUserValidator, UserValidator>();
                services.AddTransient<IFirebaseRepository, FirebaseRepository>();
            }

            return services;
        }
    }
}