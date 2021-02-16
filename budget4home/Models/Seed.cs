using System;

namespace budget4home.Models
{
    public static class Seed
    {
        public static void Run(Context context)
        {
            var user1 = new UserModel
            {
                Id = "wRUwvDbrqBbIf9DXv25QQ50ASCf2",
                Email = "silvaaavlis@gmail.com"
            };
            var user2 = new UserModel
            {
                Id = "Y615Iymw0fhWBo3wTgOazh7rdIv1",
                Email = "user@test.com"
            };

            #region group
            var group1 = context.Add(new GroupModel
            {
                Name = "Group 1"
            });
            var group2 = context.Add(new GroupModel
            {
                Name = "Group 2"
            });
            var g = context.SaveChanges();
            #endregion

            #region group user
            context.GroupUser.Add(new GroupUserModel
            {
                GroupId = group1.Entity.Id,
                UserId = user1.Id
            });
            context.GroupUser.Add(new GroupUserModel
            {
                GroupId = group1.Entity.Id,
                UserId = user2.Id
            });
            context.GroupUser.Add(new GroupUserModel
            {
                GroupId = group2.Entity.Id,
                UserId = user1.Id
            });
            var gu = context.SaveChanges();
            #endregion

            #region label
            var label1 = context.Labels.Add(new LabelModel
            {
                Name = "Label 1",
                Group = group1.Entity
            });
            var label2 = context.Labels.Add(new LabelModel
            {
                Name = "Label 2",
                Group = group1.Entity
            });
            var label3 = context.Labels.Add(new LabelModel
            {
                Name = "Label 3",
                Group = group2.Entity
            });
            var label4 = context.Labels.Add(new LabelModel
            {
                Name = "Label 4",
                Group = group2.Entity
            });
            var l = context.SaveChanges();
            #endregion

            #region expense
            var epense1 = context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Outcoming,
                Name = "Expense 1",
                Value = 100,
                Date = DateTime.Today,
                LabelId = label1.Entity.Id,
                GroupId = group1.Entity.Id
            }).Entity;
            context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Incoming,
                Name = "Expense 2",
                Value = 200,
                Date = DateTime.Today,
                LabelId = label2.Entity.Id,
                GroupId = group1.Entity.Id
            });
            context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Outcoming,
                Name = "Expense 1",
                Value = 300,
                Date = DateTime.Today.AddMonths(-1),
                LabelId = label1.Entity.Id,
                GroupId = group1.Entity.Id
            });
            context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Incoming,
                Name = "Expense 2",
                Value = 400,
                Date = DateTime.Today.AddMonths(-1),
                LabelId = label2.Entity.Id,
                GroupId = group1.Entity.Id
            });
            context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Outcoming,
                Name = "Expense 1",
                Value = 150,
                Date = DateTime.Today.AddMonths(-2),
                LabelId = label1.Entity.Id,
                GroupId = group1.Entity.Id
            });
            context.Expenses.Add(new ExpenseModel
            {
                Type = ExpenseType.Incoming,
                Name = "Expense 2",
                Value = 250,
                Date = DateTime.Today.AddMonths(-2),
                LabelId = label2.Entity.Id,
                GroupId = group1.Entity.Id
            });
            var e = context.SaveChanges();
            #endregion
        }
    }
}